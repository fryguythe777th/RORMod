using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Damaging
{
    public class ShatterspleenExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToExplosion(200, DamageClass.Generic, 20);
            Projectile.scale = 1.5f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(222, 222, 222, 200);
        }

        public override void AI()
        {
            if (Projectile.frame == 0 && Main.netMode != NetmodeID.Server)
            {
                if (Projectile.frameCounter == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item14.WithVolumeScale(0.5f), Projectile.Center);
                    SoundEngine.PlaySound(SoundID.NPCDeath1.WithPitchOffset(-0.65f).WithVolumeScale(1.25f), Projectile.Center);
                }
                for (int i = 0; i < 135; i++)
                {
                    var v = Main.rand.NextVector2Unit();
                    var d = Dust.NewDustPerfect(Projectile.Center + v * Projectile.Size / 4f * Main.rand.NextFloat(), Main.rand.NextBool() ? DustID.Blood : DustID.FoodPiece, v * Main.rand.NextFloat(1f, 7.5f) + new Vector2(0f, 4f), 60,
                        new Color(255, 85, 25), Main.rand.NextFloat(0.4f, 2f));
                    if (Main.rand.NextBool())
                    {
                        d.noGravity = true;
                        d.scale /= 2f;
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    var v = Main.rand.NextVector2Unit(i / 16f * MathHelper.TwoPi, 1f / 16f * MathHelper.TwoPi);
                    var d = Dust.NewDustPerfect(Projectile.Center, DustID.Blood, v * Main.rand.NextFloat(9f, 12f), 0,
                        new Color(255, 85, 25), Main.rand.NextFloat(1f, 2f));
                    d.noGravity = Main.rand.NextBool();
                }
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 2)
            {
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