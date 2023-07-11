using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Misc
{
    public class NkuhanaSkull : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }

        public int dustCooldown = 10;

        public override void AI()
        {
            int closest = Projectile.FindTargetWithLineOfSight();

            if (closest != -1 && Main.npc[closest].active && Main.npc[closest] != null && Main.npc[closest].life > 0)
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (Main.npc[closest].Center - Projectile.Center).SafeNormalize(-Vector2.UnitY) * 12f, 0.06f);
            }

            if (dustCooldown > 0)
            {
                dustCooldown--;
            }
            else
            {
                dustCooldown = 10;
                Dust.NewDust(Projectile.Center, 1, 1, DustID.Torch, SpeedX: -Projectile.velocity.X / 5, SpeedY: -Projectile.velocity.Y / 5);
            }
            Lighting.AddLight(Projectile.Center, TorchID.Torch);

            Projectile.scale = 1 + Projectile.damage / 100;

            if (Math.Sign(Projectile.velocity.X) == 1)
            {
                Projectile.spriteDirection = -1;
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            else
            {
                Projectile.spriteDirection = 1;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
            }
        }
    }
}