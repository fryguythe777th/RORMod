using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
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
            Projectile.tileCollide = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Main.NewText("sharing is kjaring");
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 240);
        }
    }
}