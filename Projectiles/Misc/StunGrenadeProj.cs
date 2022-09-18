using Microsoft.Xna.Framework;
using RORMod.Buffs.Debuff;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Projectiles.Misc
{
    public class StunGrenadeProj : ModProjectile
    {
        public override string Texture => RORMod.BlankTexture;

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 120;
            Projectile.hide = true;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            if (Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly)
                    {
                        Main.npc[i].AddBuff(BuffID.Confused, 60);
                        Main.npc[i].AddBuff(ModContent.BuffType<StunGrenadeDebuff>(), 60);
                    }
                }
            }

            Projectile.timeLeft -= Main.player[Projectile.owner].ownedProjectileCounts[Type] - 1;
            if (Main.netMode == NetmodeID.Server)
                return;

            if (Projectile.localAI[0] < 1f)
            {
                Projectile.localAI[0] += 0.05f;
                if (Projectile.localAI[0] >= 1f)
                {
                    Projectile.localAI[0] = 0.2f;
                }
            }

            for (int i = 0; i < 1; i++)
            {
                var normal = Main.rand.NextVector2Unit() * Projectile.localAI[0];
                var g = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center + normal * Projectile.Size / 2f * Main.rand.NextFloat(), normal.RotatedBy(Main.rand.NextFloat(MathHelper.PiOver4, MathHelper.PiOver2)), GoreID.Smoke1 + Main.rand.Next(3), Main.rand.NextFloat(0.6f, 1.2f));
                g.alpha = 120;
                g.velocity.Y += 2f;
                g.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            }

            for (int i = 0; i < 4; i++)
            {
                var normal = Main.rand.NextVector2Unit() * Projectile.localAI[0];
                var d = Dust.NewDustPerfect(Projectile.Center + normal * Projectile.Size / 2f * Main.rand.NextFloat(), DustID.Smoke, normal.RotatedBy(Main.rand.NextFloat(MathHelper.PiOver4, MathHelper.PiOver2)), Alpha: 120, Scale: Main.rand.NextFloat(0.6f, 1.2f));
            }
        }
    }
}