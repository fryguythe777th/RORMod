using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Damaging
{
    public class AtGMissileExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToExplosion(200, DamageClass.Generic, 20);
            Projectile.usesLocalNPCImmunity = false;
            Projectile.scale = 0.8f;
            Projectile.ROR().procRate = 0f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(180, 180, 180, 0);
        }

        public override void AI()
        {
            if (Projectile.frame == 0 && Main.netMode != NetmodeID.Server)
            {
                Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                for (int i = 0; i < 30; i++)
                {
                    var v = Main.rand.NextVector2Unit();
                    var d = Dust.NewDustPerfect(Projectile.Center + v * Projectile.Size / 4f * Main.rand.NextFloat(), Main.rand.NextBool(4) ? DustID.Smoke : DustID.Torch, v * Main.rand.NextFloat(3f, 12f), 0,
                        Scale: Main.rand.NextFloat(0.4f, 2.5f));
                    d.noGravity = true;
                    if (Main.rand.NextBool())
                    {
                        d.scale /= 2f;
                    }
                }
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 2)
            {
                Projectile.friendly = false;
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Type])
                {
                    Projectile.hide = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetDrawInfo(out var texture, out var offset, out var frame, out var origin, out int _);
            Main.spriteBatch.Draw(texture, Projectile.position + offset - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}