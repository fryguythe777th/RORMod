using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Content.Elites;
using RiskOfTerrain.NPCs;
using RiskOfTerrain.Projectiles.Elite;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.Aspects
{
    public class MendingAspect : GenericAspect
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Green;
        }

        public static int? savedTarget = null; //who it is currently healing
        public static int savedTargetType = 0; //0 if player, 1 if NPC
        public static int savedLifeDiff = 0; //how unhealthy the saved target is
        public static int associatedChain = -1; //the chain-drawing proj associated with this individual
        public static int healCooldown = 60; //how long until it will heal the guy

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            MendingUpdate(player);
        }

        public static void MendingUpdate(Player player)
        {
            player.ROR().aspMending = true; //makes it unable to be healed by others

            if (associatedChain >= 0)
            {
                if (!Main.projectile[associatedChain].active || Main.projectile[associatedChain].type != ModContent.ProjectileType<MendingAspectMender>())
                {
                    associatedChain = -1;
                }
            }

            if (associatedChain == -1) //summon a chain handler if it doesnt already have one
            {
                associatedChain = Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<MendingAspectMender>(), 0, 0, player.whoAmI);
            }

            //beginning of the saved target selection process
            savedTarget = null; //clean slate
            savedLifeDiff = 0;  //~~~

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC mendTarget = Main.npc[i];

                if (mendTarget.active //ensure that the potential target is alive, in range, an enemy, not mending, and not itself
                    && mendTarget.Distance(player.Center) <= 800
                    && mendTarget.friendly
                    && !mendTarget.ROR().isMending
                    && mendTarget.lifeMax - mendTarget.life > savedLifeDiff) //sees if this is the enemy with the lowest health
                {
                    savedTarget = i; //makes this the optimal new target
                    savedLifeDiff = mendTarget.lifeMax - mendTarget.life; //and sets a new unhealthiness reference value
                    savedTargetType = 1;
                }
            }

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player mendTarget = Main.player[i];

                if (mendTarget.active
                    && mendTarget.Distance(player.Center) <= 800
                    && mendTarget.team == player.team
                    && mendTarget.ROR().aspMending
                    && mendTarget.statLifeMax2 - mendTarget.statLife > savedLifeDiff)
                {
                    savedTarget = i;
                    savedLifeDiff = mendTarget.statLifeMax2 - mendTarget.statLife;
                    savedTargetType = 0;
                }
            }

            Projectile chain = Main.projectile[associatedChain];

            if (savedTarget != null && savedLifeDiff > 0)
            {
                if (healCooldown == 0)
                {
                    if (savedTargetType == 0)
                    {
                        Main.player[(int)savedTarget].statLife++;
                    }
                    else
                    {
                        Main.npc[(int)savedTarget].life++;
                    }

                    if (NPC.downedPlantBoss)
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

                if (savedTargetType == 0)
                {
                    chain.ai[0] = Main.player[(int)savedTarget].whoAmI;
                }
                else
                {
                    chain.ai[0] = Main.npc[(int)savedTarget].whoAmI;
                }
                chain.ai[1] = 1;
                chain.ai[2] = savedTargetType;
            }
            else
            {
                chain.ai[0] = player.whoAmI;
                chain.ai[1] = 0;
                chain.ai[2] = 0;
            }
        }

        public override void Hurt(Player player, RORPlayer ror, Player.HurtInfo info)
        {
            MendingHurt(player, info);
        }

        public static void MendingHurt(Player player, Player.HurtInfo info)
        {
            if (info.Damage > player.statLifeMax * 0.25)
            {
                Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<MendingBomb>(), 0, 0);
            }
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight(Item.Center, TorchID.Green);
        }
    }

    public class MendingAspectMender : ModProjectile
    {
        public override string Texture => "RiskOfTerrain/Items/Accessories/T1Common/BustlingFungus";
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
            if (!Main.player[Projectile.owner].active || !Main.player[Projectile.owner].ROR().aspMending)
            {
                Projectile.Kill();
            }

            if (Projectile.ai[2] == 0)
            {
                Projectile.Center = Main.player[(int)Projectile.ai[0]].Center;
            }
            else
            {
                Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
            }
        }

        public override bool PreDrawExtras()
        {
            if (Projectile.ai[1] == 1)
            {
                Vector2 eliteCenter = Main.player[Projectile.owner].Center;
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