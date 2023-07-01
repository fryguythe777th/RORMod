using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Misc
{
    public class WilloExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 9;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 35;
            Projectile.knockBack = 5f;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.width = 68;
            Projectile.height = 151;
            Projectile.tileCollide = false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(150, 220, 65, 230);
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 2)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Type])
                {
                    Projectile.Kill();
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