using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Damaging
{
    public class RazorwireProj : ModProjectile
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

        public override void AI()
        {
            var closest = Main.npc[(int)Projectile.ai[0]];
            if (!closest.active)
            {
                Projectile.Kill();
            }

            Projectile.velocity = Vector2.Lerp(Projectile.velocity, (closest.Center - Projectile.Center).SafeNormalize(-Vector2.UnitY) * 12f, 0.06f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }
}