using System;
using System.Security.AccessControl;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Visual
{
    public class MeatHookProjVanity : ModProjectile
    {
        private static Asset<Texture2D> chainTexture;
        private static Asset<Texture2D> hookTexture;
        public bool flipped;

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
            Projectile.width = 18;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.timeLeft = 2;

            if (!player.ROR().showMeatHook)
            {
                Projectile.Kill();
            }

            if (player.direction == 1)
            {
                Projectile.velocity = new Vector2(((player.Center.X - 40) - Projectile.Center.X) / 9, ((player.Center.Y - 5) - Projectile.Center.Y) / 9);
            }

            if (player.direction == -1)
            {
                Projectile.velocity = new Vector2(((player.Center.X + 30) - Projectile.Center.X) / 9, ((player.Center.Y - 5) - Projectile.Center.Y) / 9);
            }

            Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 directionToPlayer = playerCenter - Projectile.Center;
            Projectile.rotation = directionToPlayer.ToRotation() - MathHelper.Pi;

            if (Projectile.Center.X < player.Center.X)
            {
                flipped = true;
            }
            else
            {
                flipped = false;
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
            Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 center = Projectile.Center;
            Vector2 directionToPlayer = playerCenter - Projectile.Center;
            float chainRotation = directionToPlayer.ToRotation() - MathHelper.PiOver2;
            float distanceToPlayer = directionToPlayer.Length();

            while (distanceToPlayer > 20f && !float.IsNaN(distanceToPlayer))
            {
                directionToPlayer /= distanceToPlayer;
                directionToPlayer *= chainTexture.Height();

                center += directionToPlayer;
                directionToPlayer = playerCenter - center;
                distanceToPlayer = directionToPlayer.Length();

                Color drawColor = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16));

                Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                    chainTexture.Value.Bounds, drawColor, chainRotation,
                    chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}