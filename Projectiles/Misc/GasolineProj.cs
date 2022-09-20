using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Projectiles.Misc
{
    public class GasolineProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Projectile.alpha == 0)
            {
                Projectile.scale = 0.1f;
            }
            Projectile.scale += 0.15f - Projectile.scale * 0.15f;
            Projectile.scale *= 1.1f;
            Projectile.alpha += 12;
            int size = (int)(Projectile.width * Projectile.scale);
            var rect = new Rectangle((int)Projectile.position.X + Projectile.width / 2 - size, (int)Projectile.position.Y + Projectile.height / 2 - size, size * 2, size * 2);
            if (Projectile.alpha < 100)
            {
                for (int i = 0; i < 16 * Projectile.scale; i++)
                {
                    var normal = Main.rand.NextVector2Unit();
                    var d = Dust.NewDustPerfect(Projectile.Center + normal * size * 0.7f * Main.rand.NextFloat(0.5f, 0.8f), DustID.Torch, normal.RotatedBy(Main.rand.NextFloat(0.1f)) * Main.rand.NextFloat(0.5f, 1f) * 6f, Scale: Main.rand.NextFloat(0.6f, 4f) * Projectile.scale);
                    d.noGravity = true;
                }
            }
            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].CanBeChasedBy(Projectile) && Projectile.Colliding(rect, Main.npc[i].Hitbox))
                {
                    Main.npc[i].AddBuff(BuffID.OnFire, 300);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float opacity = (float)Math.Sin(Projectile.Opacity * MathHelper.Pi);
            lightColor = Color.White * opacity * 0.5f;
            lightColor.A = 0;
            Projectile.GetDrawInfo(out var t, out var off, out var frame, out var origin, out int _);

            Main.EntitySpriteDraw(t, Projectile.position + off - Main.screenPosition, frame, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(t, Projectile.position + off - Main.screenPosition, frame, lightColor * 0.8f * opacity, Projectile.rotation, origin, Projectile.scale * 1.15f, SpriteEffects.None, 0);
            return false;
        }
    }
}