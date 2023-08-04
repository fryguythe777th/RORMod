using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Damaging
{
    public class ResonanceDiscProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
        }

        public int decayCounter = 300;
        public int mostRecentHit = -1;
        public int state = 1;
        //state one: resting
        //state two: charging up
        //state three: attacking

        public override void AI()
        {
            Projectile.timeLeft = 2;
            int charge = (int)Projectile.ai[0] + 1;
            Player player = Main.player[Projectile.owner];

            if (state == 1)
            {
                if (player.direction == 1)
                {
                    Projectile.velocity = new Vector2(((player.Center.X - 60) - Projectile.Center.X) / 7, ((player.Center.Y - 10) - Projectile.Center.Y) / 7);
                    Projectile.rotation -= MathHelper.ToRadians(charge * 3);
                }

                if (player.direction == -1)
                {
                    Projectile.velocity = new Vector2(((player.Center.X + 60) - Projectile.Center.X) / 7, ((player.Center.Y - 10) - Projectile.Center.Y) / 7);
                    Projectile.rotation += MathHelper.ToRadians(charge * 4);
                }

                if (charge >= 7)
                {
                    Projectile.ai[0] = 0;
                    state = 2;
                }

                if (decayCounter > 0)
                {
                    decayCounter--;
                }
                else
                {
                    decayCounter = 300;
                    if (Projectile.ai[0] > 0)
                    {
                        Projectile.ai[0] -= 1;
                    }
                }
            }

            if (state == 2)
            {
                Projectile.rotation -= MathHelper.ToRadians(21);
                int probableTarget = -1;
                int savedDistance = 100000;

                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.penetrate = -1;
                Projectile.damage = 30;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Projectile.Distance(Main.npc[i].Center) < savedDistance && i != mostRecentHit && Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].lifeMax > 5 && Main.npc[i].damage > 0)
                    {
                        probableTarget = i;
                        savedDistance = (int)Projectile.Distance(Main.npc[i].Center);
                    }
                }

                if (probableTarget > -1)
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, (Main.npc[probableTarget].Center - Projectile.Center).SafeNormalize(-Vector2.UnitY) * 12f, 0.2f);
                }
                else
                {
                    Projectile.Kill();
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            mostRecentHit = target.whoAmI;

            if (!target.active)
            {
                Projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Main.player[Projectile.owner].ROR().accResonanceDisc)
            {
                int i = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileID.Grenade, 30, 2, Projectile.owner);
                Main.projectile[i].timeLeft = 2;
                Main.player[Projectile.owner].ROR().resDiscID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Main.player[Projectile.owner].Center, Vector2.Zero, ModContent.ProjectileType<ResonanceDiscProj>(), 0, 0, Projectile.owner);
            }
        }
    }
}