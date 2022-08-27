using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Projectiles.Misc
{
    public class HealingOrb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.alpha = 0;
            Projectile.scale = 0.5f;
        }

        private int blinkCounter = 0;

        public override void AI()
        {
            Projectile.velocity.Y += 0.05f;

            /*for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Projectile.Hitbox.Intersects(Main.projectile[i].Hitbox))
                {
                    Projectile.velocity += Main.projectile[i].velocity;
                }
            }*/

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Projectile.Hitbox.Intersects(Main.player[i].Hitbox))
                {
                    Main.player[i].Heal(8 + (int)(Main.player[i].statLifeMax * 0.02));
                    Projectile.Kill();
                }
            }

            if (blinkCounter >= (Math.Round((double)(Projectile.timeLeft / 150)) + 1) * 5)
            {
                blinkCounter = 0;
                if (Projectile.alpha == 95)
                {
                    Projectile.alpha = 0;
                }
                else
                {
                    Projectile.alpha = 95;
                }
            }

            blinkCounter++;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.Y = -oldVelocity.Y * 0.69f;
            return false;
        }
    }
}