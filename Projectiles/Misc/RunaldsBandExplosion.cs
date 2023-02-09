using System;
using Microsoft.Xna.Framework;
using RiskOfTerrain.Buffs.Debuff;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Misc
{
    public class RunaldsBandExplosion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.damage = 0;
        }

        public bool dustToggle = false;
        public int killCounter = 0;

        public override void AI()
        {
            for (int i = 0; i < 24; i++)
            {
                int dustType;

                if (dustToggle)
                {
                    dustType = 197;
                    dustToggle = false;
                }
                else
                {
                    dustType = 312;
                    dustToggle = true;
                }

                Vector2 dustVelocity = new Vector2(0, 3).RotatedBy(MathHelper.ToRadians(i * 15));
                Dust.NewDust(Projectile.Center, 2, 2, dustType, dustVelocity.X, dustVelocity.Y);
            }

            if (killCounter > 1)
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.scale = 7f;
                Projectile.alpha = 255;
                Projectile.damage = 35;
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode);
                killCounter++;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (damage > 0)
            {
                target.AddBuff(ModContent.BuffType<RunaldFreeze>(), 180);
            }
        }
    }
}