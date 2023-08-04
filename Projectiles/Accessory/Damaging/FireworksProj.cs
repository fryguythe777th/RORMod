using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Damaging
{
    public class FireworksProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Generic;
            Projectile.width = 12;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if ((int)Projectile.localAI[0] == 0)
            {
                SoundEngine.PlaySound(RiskOfTerrain.GetSounds("firework/shoot", 10, 0.1f, 0f, 0.1f), Projectile.Center);
                Projectile.localAI[0] = 1f;
            }
            if (!Main.dedServ && Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.Center, 2, 2, DustID.Torch, -Projectile.velocity.X / 2, -Projectile.velocity.Y / 2);
            }

            for (int i = 0; i < 2; i++)
            {
                var d = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, Main.rand.NextBool(4) ? DustID.Smoke : DustID.Torch,
                    Scale: Main.rand.NextFloat(0.4f, 2f));
                d.velocity *= 0.5f;
                d.velocity += -Projectile.velocity * Main.rand.NextFloat(0.1f, 0.2f);
                d.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, TorchID.Torch);

            if (Projectile.ai[0] > 0)
            {
                Projectile.ai[0]--;
                Projectile.penetrate = -1;
            }
            else
            {
                Projectile.penetrate = 1;
                int npcIndex = Projectile.FindTargetWithLineOfSight(800f);

                if (npcIndex == -1)
                {
                    return;
                }

                var closest = Main.npc[npcIndex];

                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (closest.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 8f, 0.08f);
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(RiskOfTerrain.GetSounds("firework/explode", 7, 0.9f, 0f, 0.1f), Projectile.Center);

            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(Main.player[Projectile.owner].GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FireworkBlast>(), 10, 4, Projectile.owner);
            }
        }
    }
}