using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Misc
{
    public class KjarosBandTornado : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 5;
            Projectile.knockBack = 2f;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.width = 78;
            Projectile.height = 96;
            Projectile.timeLeft = 840;
            Projectile.alpha = 80;
            //Projectile.scale = 2f;
            Projectile.localNPCHitCooldown = 2;
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
                    Projectile.frame = 0;
                }
            }

            if (Projectile.timeLeft <= 175)
            {
                Projectile.alpha += 175 - Projectile.timeLeft;
            }

            Lighting.AddLight(Projectile.Center, TorchID.Orange);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.EntitySpriteDraw((Texture2D)ModContent.Request<Texture2D>(Texture), Projectile.Center, null, new Color(0, 0, 0, Projectile.alpha), 0, Vector2.Zero, 4f, SpriteEffects.None);
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 240);
        }
    }
}