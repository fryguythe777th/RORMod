using System;
using System.Security.AccessControl;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.NPCs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Visual
{
    public class MeatHookProjDamaging : ModProjectile
    {
        private static Asset<Texture2D> chainTexture;
        private static Asset<Texture2D> hookTexture;
        public bool flipped;

        public override string Texture => "RiskOfTerrain/Projectiles/Accessory/Visual/MeatHookProjChain";

        public override void Load()
        {
            chainTexture = ModContent.Request<Texture2D>("RiskOfTerrain/Projectiles/Accessory/Visual/MeatHookProjChain");
            hookTexture = ModContent.Request<Texture2D>("RiskOfTerrain/Projectiles/Accessory/Visual/MeatHookProjVanity");
        }

        public override void Unload()
        {
            chainTexture = null;
            hookTexture = null;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
        }

        int state = 1;

        public override void AI() //ai0 is target, ai1 is source npc
        {
            if (!Main.npc[(int)Projectile.ai[0]].active || !Main.npc[(int)Projectile.ai[1]].active)
            {
                Projectile.Kill();
            }
            Projectile.timeLeft = 2;

            Vector2 sourceCenter = Main.npc[(int)Projectile.ai[1]].Center;
            Vector2 directionFromSource = sourceCenter - Projectile.Center;
            Projectile.rotation = directionFromSource.ToRotation() + MathHelper.Pi;

            if (Projectile.Center.X < Main.npc[(int)Projectile.ai[1]].Center.X)
            {
                flipped = true;
            }
            else
            {
                flipped = false;
            }

            Vector2 targetCenter = Main.npc[(int)Projectile.ai[0]].Center;
            if (state == 1)
            {
                Projectile.Center = Vector2.Lerp(Projectile.Center, targetCenter, 0.25f);

                if (RORNPC.Distance(Projectile, Main.npc[(int)Projectile.ai[0]]) < 10)
                {
                    state = 2;
                }
            }
            else if (state == 2)
            {
                Projectile.Center = Vector2.Lerp(Projectile.Center, sourceCenter, 0.25f);
                Main.npc[(int)Projectile.ai[0]].Center = Projectile.Center;

                if (RORNPC.Distance(Projectile, Main.npc[(int)Projectile.ai[1]]) < 30)
                {
                    Projectile.Kill();
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (flipped)
            {
                Main.EntitySpriteDraw(hookTexture.Value, Projectile.Center - Main.screenPosition, hookTexture.Value.Bounds, lightColor, Projectile.rotation, hookTexture.Size() * 0.5f, 1f, SpriteEffects.FlipVertically, 0);
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool PreDrawExtras()
        {
            Vector2 sourceCenter = Main.npc[(int)Projectile.ai[1]].Center;
            Vector2 center = Projectile.Center;
            Vector2 directionToSource = sourceCenter - Projectile.Center;
            float chainRotation = directionToSource.ToRotation() - MathHelper.PiOver2;
            float distanceToSource = directionToSource.Length();

            if (Projectile.active)
            {
                while (distanceToSource > 20f && !float.IsNaN(distanceToSource))
                {
                    directionToSource /= distanceToSource;
                    directionToSource *= chainTexture.Height();

                    center += directionToSource;
                    directionToSource = sourceCenter - center;
                    distanceToSource = directionToSource.Length();

                    Color drawColor = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16));

                    Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                        chainTexture.Value.Bounds, drawColor, chainRotation,
                        chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
                }
            }
            return false;
        }
    }
}