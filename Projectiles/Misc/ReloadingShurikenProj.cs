using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Misc
{
    public class ReloadingShurikenProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 60;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Generic;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.scale = 1.5f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = 5;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 16;
            height = 16;
            return true;
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.Length() * 0.1f * Projectile.direction;
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TintableDustLighted, -Projectile.velocity.X, -Projectile.velocity.Y, newColor: new Color(255, 50, 50, 100), Scale: Main.rand.NextFloat(0.8f, 1.66f));
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            lightColor = Color.White;
            Projectile.GetDrawInfo(out var texture, out var offset, out var frame, out var origin, out int trailLength);
            var clr = Projectile.GetAlpha(lightColor) * Projectile.Opacity;
            var trailTexture = ModContent.Request<Texture2D>($"{RiskOfTerrain.AssetsPath}LightRay3", AssetRequestMode.ImmediateLoad).Value;
            for (int i = 0; i < trailLength; i++)
            {
                float progress = (float)Math.Pow(1f - 1f / trailLength * i, 2f);
                Main.EntitySpriteDraw(trailTexture, Projectile.oldPos[i] - Vector2.Normalize(Projectile.velocity) * 24f + offset - Main.screenPosition, null, new Color(255, 50, 80,0) * Projectile.Opacity * 0.7f * progress, 
                    Projectile.velocity.ToRotation() - MathHelper.PiOver2, new Vector2(trailTexture.Width /2f, trailTexture.Height - 20f), new Vector2(Projectile.scale * 0.5f, Projectile.scale), SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture, Projectile.position + offset - Main.screenPosition, frame, clr, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}