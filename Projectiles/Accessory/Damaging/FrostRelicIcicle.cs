using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Damaging
{
    public class FrostRelicIcicle : ModProjectile
    {
        public override string Texture => "RiskOfTerrain/Projectiles/Accessory/Damaging/RunaldsBandShard";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 480;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.damage = 10;
            Projectile.knockBack = 0;
        }

        public int rotateCounter;
        public float sinEffect = 0;
        public int sinDirection = 1;

        public override void AI()
        {
            if (Projectile.timeLeft <= 51)
            {
                Projectile.alpha = 255 - Projectile.timeLeft * 5;
            }
            if (sinDirection == 1)
            {
                sinEffect = MathHelper.Lerp(sinEffect, 20f, 0.02f);
                if (sinEffect > 16f)
                {
                    sinDirection = -1;
                }
            }
            else
            {
                sinEffect = MathHelper.Lerp(sinEffect, -20f, 0.02f);
                if (sinEffect < -16f)
                {
                    sinDirection = 1;
                }
            }

            Projectile.frame = 3;
            rotateCounter++;
            int distanceFromPlayer = (int)sinEffect + 80;
            Player player = Main.player[Projectile.owner];

            Vector2 playerCenter = new Vector2(player.Center.X - 5, player.Center.Y - 5);

            Projectile.position = playerCenter + new Vector2(0, distanceFromPlayer).RotatedBy(MathHelper.ToRadians(rotateCounter + (120 * Projectile.ai[0])));

            
            Vector2 directionToPlayer = playerCenter - Projectile.Center;
            Projectile.rotation = directionToPlayer.ToRotation() - MathHelper.PiOver2;
        }
    }
}