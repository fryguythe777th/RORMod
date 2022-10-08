using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Misc
{
    public class RazorwireProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 400;
            Projectile.tileCollide = false;
            Projectile.width = 6;
            Projectile.height = 24;
            Projectile.penetrate = 1;
        }

        public override void AI()
        {
            var closest = Main.npc[(int)Projectile.ai[0]];
            if (!closest.active)
            {
                Projectile.Kill();
            }

            Projectile.velocity = Vector2.Lerp(Projectile.velocity, (closest.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 6f, 0.1f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }
}