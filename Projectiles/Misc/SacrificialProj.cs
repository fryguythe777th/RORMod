using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
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
            Projectile.width = 10;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 3000;
            Projectile.alpha = 130;
            Projectile.tileCollide = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(0, 3);
        }

        public bool tileCollisionActive;

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
                Projectile.velocity = new Vector2(0, 0.1f);

                tileCollisionActive = true;
            }
            else
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (closest.Center - Projectile.Center).SafeNormalize(-Vector2.UnitY) * 12f, 0.06f);
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                tileCollisionActive = false;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (tileCollisionActive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(2, 6); i++)
            {
                Dust.NewDust(Projectile.Center, 2, 2, DustID.Smoke, Main.rand.Next(-1, 2), Main.rand.Next(-1, 2), 125, new Color(255, 30, 259));
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 30, 259) * Projectile.Opacity;
        }
    }
}