using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
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

        public int? savedTarget = null;
        public int savedLifeDiff = 0;
        public int associatedChain = 0;
        public int healCooldown = 45;

        public override void OnBecomeElite(NPC npc)
        {
            associatedChain = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<MendingMender>(), 0, 0, npc.whoAmI, ai0: npc.whoAmI);
        }

        public override void AI(NPC npc)
        {
            if (active)
            {
                savedTarget = null;
                savedLifeDiff = 0;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC mendTarget = Main.npc[i];
                    if (mendTarget.active && mendTarget.Distance(npc.Center) <= 800 && !mendTarget.friendly)
                    {
                        if (mendTarget.lifeMax - mendTarget.life > savedLifeDiff)
                        {
                            savedTarget = i;
                            savedLifeDiff = mendTarget.lifeMax - mendTarget.life;
                        }
                    }
                }

                Projectile chain = Main.projectile[associatedChain];

                if (savedTarget != null && savedLifeDiff > 0 && savedTarget != npc.whoAmI)
                {
                    if (healCooldown == 0)
                    {
                        Main.npc[(int)savedTarget].life++;
                        healCooldown = 45;
                    }
                    else
                    {
                        healCooldown--;
                    }
                    chain.ai[0] = Main.npc[(int)savedTarget].whoAmI;
                    chain.ai[1] = 1; 
                }
                else
                {
                    chain.ai[0] = npc.whoAmI;
                    chain.ai[1] = 0;
                }
            }
        }

        public override bool CanRoll(NPC npc)
        {
            return true;
        }
    }

    public class MendingMender : ModProjectile
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

        public override void OnSpawn(IEntitySource source)
        {
            Main.NewText("wtfwtfwtf");
        }

        public override void AI()
        {
            if (!Main.npc[Projectile.owner].active)
            {
                Projectile.Kill();
            }

            Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
        }

        public override bool PreDrawExtras()
        {
            if (Projectile.ai[1] == 1)
            {
                Vector2 eliteCenter = Main.npc[Projectile.owner].Center;
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

                    Color drawColor = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16));

                    Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                        chainTexture.Value.Bounds, drawColor, chainRotation,
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