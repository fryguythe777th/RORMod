using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Items.Accessories.Aspects;
using RiskOfTerrain.Projectiles.Elite;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Elites
{
    public class MendingElite : EliteNPCBase
    {
        public override ArmorShaderData Shader => GameShaders.Armor.GetShaderFromItemId(ItemID.GreenDye);

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public int? savedTarget = null; //who it is currently healing
        public int savedLifeDiff = 0; //how unhealthy the saved target is
        public int associatedChain = -1; //the chain-drawing proj associated with this individual
        public int healCooldown = 60; //how long until it will heal the guy

        public override void AI(NPC npc)
        {
            if (active)
            {
                if (associatedChain == -1) //summon a chain handler if it doesnt already have one
                {
                    associatedChain = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<MendingMender>(), 0, 0);
                }
                Main.projectile[associatedChain].ai[2] = npc.whoAmI; //tell it who it belongs to

                npc.ROR().isMending = true; //makes it unable to be healed by others

                //beginning of the saved target selection process
                savedTarget = null; //clean slate
                savedLifeDiff = 0;  //~~~

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC mendTarget = Main.npc[i];

                    if (mendTarget.active //ensure that the potential target is alive, in range, an enemy, not mending, and not itself
                        && mendTarget.Distance(npc.Center) <= 800 
                        && !mendTarget.friendly 
                        && !mendTarget.ROR().isMending
                        && mendTarget.lifeMax - mendTarget.life > savedLifeDiff //sees if this is the enemy with the lowest health
                        && mendTarget.whoAmI != npc.whoAmI)
                    {
                        savedTarget = i; //makes this the optimal new target
                        savedLifeDiff = mendTarget.lifeMax - mendTarget.life; //and sets a new unhealthiness reference value
                    }
                }
                //end of selection

                //healing and chain drawing info
                Projectile chain = Main.projectile[associatedChain];

                if (savedTarget != null && savedLifeDiff > 0) //if there was an actual target selected in the above process
                {
                    if (healCooldown == 0)
                    {
                        Main.npc[(int)savedTarget].life++; //heal him

                        if (NPC.downedPlantBoss) //reset the timer based on game progress
                        {
                            healCooldown = 15;
                        }
                        else if (Main.hardMode)
                        {
                            healCooldown = 30;
                        }
                        else
                        {
                            healCooldown = 60;
                        }
                    }
                    else
                    {
                        healCooldown--;
                    }
                    chain.ai[0] = Main.npc[(int)savedTarget].whoAmI; //makes the chain end at the heal target
                    chain.ai[1] = 1; //tells the chain that it should draw
                }
                else
                {
                    chain.ai[0] = npc.whoAmI; //makes the chain return to the individual
                    chain.ai[1] = 0; //tells the chain not to draw
                }
            }
        }

        public override bool PreKill(NPC npc)
        {
            if (active)
            {
                Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, Vector2.Zero, ModContent.ProjectileType<MendingBomb>(), 0, 0);
            }
            return true;
        }

        public override bool CanRoll(NPC npc)
        {
            return !ServerConfig.Instance.MendingElitesDisable;
        }

        public override void OnBecomeElite(NPC npc)
        {
            npc.lifeMax = (int)(npc.lifeMax * 1.5f);
            npc.life = (int)(npc.life * 1.5f);
            npc.npcSlots *= 3f;
            npc.value *= 2;
        }

        public override void OnKill(NPC npc)
        {
            if (active)
            {
                int rollNumber = npc.boss ? 1000 : 4000;
                if (Main.player[Player.FindClosest(npc.Center, 500, 500)].RollLuck(rollNumber) == 0)
                {
                    int i = Item.NewItem(npc.GetSource_GiftOrReward(), npc.Center, ModContent.ItemType<MendingAspect>());
                    Main.item[i].velocity = new Vector2(0, -4);
                }
            }
        }
    }

    public class MendingMender : ModProjectile
    {
        public override string Texture => "RiskOfTerrain/Items/Accessories/T1Common/BustlingFungus"; //bungus
        private static Asset<Texture2D> chainTexture;

        public override void Load()
        {
            chainTexture = ModContent.Request<Texture2D>("RiskOfTerrain/Projectiles/Elite/MendingChain");
        }

        public override void Unload()
        {
            chainTexture = null;
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (!Main.npc[(int)Projectile.ai[2]].active || !Main.npc[(int)Projectile.ai[2]].ROR().isMending) //dies when the individual dies
            {
                Projectile.Kill();
            }

            Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center; //go to whomever it needs to follow at the moment
        }

        public override bool PreDrawExtras() //basic chain drawing code if it is proper drawing time
        {
            if (Projectile.ai[1] == 1)
            {
                Vector2 eliteCenter = Main.npc[(int)Projectile.ai[2]].Center;
                Vector2 center = Projectile.Center;
                Vector2 directionToElite = eliteCenter - Projectile.Center;
                float chainRotation = directionToElite.ToRotation() - MathHelper.PiOver2;
                float distanceToElite = directionToElite.Length();

                while (distanceToElite > 20f && !float.IsNaN(distanceToElite))
                {
                    directionToElite /= distanceToElite;
                    directionToElite *= chainTexture.Height();

                    center += directionToElite;
                    directionToElite = eliteCenter - center;
                    distanceToElite = directionToElite.Length();

                    Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                        chainTexture.Value.Bounds, Color.LightGreen, chainRotation,
                        chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
                }
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}