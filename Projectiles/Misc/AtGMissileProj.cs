using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Misc
{
    public class AtGMissileProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Generic;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 2;
            Projectile.ROR().procRate = 0f;
        }

        public override void AI()
        {
            if ((int)Projectile.localAI[0] == 0)
            {
                SoundEngine.PlaySound(RiskOfTerrain.GetSounds("missile/shoot", 4, 0.25f, 0f, 0.1f), Projectile.Center);
                Projectile.localAI[0] = 1f;
            }
            if (Projectile.penetrate <= 1)
            {
                Projectile.Kill();
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

            Projectile.rotation = Projectile.velocity.ToRotation();

            int npcIndex = Projectile.FindTargetWithLineOfSight(800f);

            if (npcIndex == -1)
            {
                return;
            }

            Projectile.ai[0]++;
            var closest = Main.npc[npcIndex];
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, (closest.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 16f, Math.Clamp(0.01f + Projectile.ai[0] / 180f, 0f, 1f));
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(RiskOfTerrain.GetSounds("missile/explode", 4, 0.5f, 0f, 0.1f), Projectile.Center);

            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AtGMissileExplosion>(), Projectile.damage, 1f, Projectile.owner);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetDrawInfo(out var t, out var off, out var frame, out var origin, out int _);
            Main.EntitySpriteDraw(t, Projectile.position + off - Main.screenPosition, frame, lightColor, Projectile.rotation + MathHelper.PiOver2, origin, Projectile.scale, SpriteEffects.None, 0);

            var flarePos = Projectile.position + off - (Projectile.rotation).ToRotationVector2() * (t.Height / 2f - 2f);
            Main.instance.LoadProjectile(ProjectileID.RainbowCrystalExplosion);
            t = TextureAssets.Projectile[ProjectileID.RainbowCrystalExplosion].Value;
            frame = t.Frame();
            origin = frame.Size() / 2f;
            Main.EntitySpriteDraw(t, flarePos - Main.screenPosition, frame, new Color(255, 180, 20, 180), 0f, origin, Projectile.scale * 0.5f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(t, flarePos - Main.screenPosition, frame, new Color(255, 180, 20, 180), MathHelper.PiOver2, origin, new Vector2(Projectile.scale * 0.5f, Projectile.scale * 2f), SpriteEffects.None, 0);
            return false;
        }
    }
}