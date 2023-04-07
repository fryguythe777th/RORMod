using System;
using Microsoft.Xna.Framework;
using RiskOfTerrain.Buffs.Debuff;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
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
            Projectile.penetrate = -1;
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

                for (int i = 0; i < Main.rand.Next(5, 10); i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 5).RotatedByRandom(6.2), ModContent.ProjectileType<RunaldsBandShard>(), 0, 0);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Damage > 0)
            {
                target.AddBuff(ModContent.BuffType<RunaldFreeze>(), 180);
            }
        }
    }

    public class RunaldsBandShard : ModProjectile
    {
        //projectile that has a random size, lifetime depends on size, is shot out in a random direction a random amount of times, applies freeze

        public override void SetStaticDefaults()
        {
            Main.projFrames[this.Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 46;
            Projectile.friendly = true;
            Projectile.damage = 20;
            Projectile.penetrate = -1; 
        }

        public int Size;

        public override void OnSpawn(IEntitySource source)
        {
            Size = Main.rand.Next(1, 5);

            if (Size == 1)
            {
                Projectile.frame = 4;
                Projectile.timeLeft = 18;
            }
            else if (Size == 2)
            {
                Projectile.frame = 3;
                Projectile.timeLeft = 24;
            }
            else if (Size == 3)
            {
                Projectile.frame = 2;
                Projectile.timeLeft = 30;
            }
            else
            {
                Projectile.frame = 1;
                Projectile.timeLeft = 36;
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() +  MathHelper.PiOver2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<RunaldFreeze>(), 180);
        }
    }
}