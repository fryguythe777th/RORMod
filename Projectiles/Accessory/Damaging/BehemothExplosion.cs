using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Damaging
{
    public class BehemothExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 1;
            Projectile.knockBack = 15f;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 194, 95, 200);
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Main.rand.Next(0, 360);

            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.netUpdate = true;
            }
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