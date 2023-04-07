using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Misc
{
    public class SacrificialProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 3000;
            Projectile.tileCollide = false;
            Projectile.alpha = 130;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(0, 3);
        }

        public override void AI()
        {
            int target = Projectile.FindTargetWithLineOfSight();
            NPC closest = null;

            if (target != -1)
            {
                closest = Main.npc[target];
            }

            if (closest == null || !closest.active || closest.friendly || closest.lifeMax == 5 || closest.damage == 0)
            {
                Projectile.velocity = Vector2.Zero;
            }
            else
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (closest.Center - Projectile.Center).SafeNormalize(-Vector2.UnitY) * 12f, 0.06f);
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 30, 259) * Projectile.Opacity;
        }
    }
}