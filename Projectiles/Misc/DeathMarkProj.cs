using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace RORMod.Projectiles.Misc
{
    public class DeathMarkProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 14;
        }


        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(222, 222, 222, 200);
        }

        public override void AI()
        {
            Projectile.velocity *= 0.85f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 3)
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