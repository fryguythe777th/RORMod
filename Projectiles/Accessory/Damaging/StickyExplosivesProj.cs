using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Damaging
{
    public class StickyExplosivesProj : ModProjectile
    {
        public int AttatchedNPC { get => (int)Projectile.ai[0]; }

        public bool playedSound;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (!playedSound)
            {
                SoundEngine.PlaySound(RiskOfTerrain.GetSounds("stickybomb_", 3, volume: 0.3f), Projectile.Center);
                playedSound = true;
            }

            if (AttatchedNPC == -1 || !Main.npc[AttatchedNPC].active)
            {
                Projectile.Kill();
                Projectile.ai[0] = -1f;
                if (Projectile.ai[1] != 0f)
                {
                    Projectile.ai[1] = 0f;
                    Projectile.velocity = Main.rand.NextVector2Unit() * 3f;
                    Projectile.netUpdate = true;
                }
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                Projectile.velocity.Y += 0.1f;
                Projectile.tileCollide = true;
                return;
            }
            var npcCenter = Main.npc[AttatchedNPC].Center;
            if (Projectile.ai[1] == 0f)
            {
                Projectile.ai[1] = Vector2.Normalize(npcCenter - Projectile.Center).ToRotation();
            }
            var normal = Projectile.ai[1].ToRotationVector2();
            Projectile.Center = npcCenter - (normal * (Main.npc[AttatchedNPC].frame.Size() / 2f) - normal * 10f).RotatedBy(Main.npc[AttatchedNPC].rotation);
            Projectile.rotation = Projectile.ai[1] + Main.npc[AttatchedNPC].rotation + MathHelper.PiOver2;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > Projectile.timeLeft / 4)
            {
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
                if (Projectile.frame == 1)
                    SoundEngine.PlaySound(RiskOfTerrain.GetSound("stickybombtick", volume: 0.3f), Projectile.Center);

                Projectile.frameCounter = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            var velo = Vector2.Zero;
            if (AttatchedNPC != -1)
            {
                velo = new Vector2(-Main.npc[AttatchedNPC].direction * 0.01f, 0f);
            }
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, velo, ModContent.ProjectileType<StickyExplosivesExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= 0.9f;
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
    }
}