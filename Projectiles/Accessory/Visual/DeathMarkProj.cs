using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Visual
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
            if ((int)Projectile.ai[0] >= 0)
            {
                var npc = Main.npc[(int)Projectile.ai[0]];
                Projectile.Center = new Vector2(npc.position.X + npc.width / 2f, npc.position.Y - 50f);
            }
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