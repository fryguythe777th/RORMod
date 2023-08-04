using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Items.CharacterSets.Artificer;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Commando
{
    public class DoubleTapPierce : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 18;
            Projectile.penetrate = 5;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.damage = 1;
            Projectile.alpha = 100;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * 1.4f);
        }
    }

    public class GrenadeProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = -1;

            Projectile.timeLeft = 300;

            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.expertMode)
            {
                if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail)
                {
                    modifiers.FinalDamage /= 5;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[1] != 0)
            {
                return true;
            }

            if (Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            }
            Projectile.soundDelay = 10;

            if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f)
            {
                Projectile.velocity.X = oldVelocity.X * -0.5f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f)
            {
                Projectile.velocity.Y = oldVelocity.Y * -0.5f;
            }
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }

        public override void AI()
        {
            Projectile.damage = 0;
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
            dust.scale = 0.1f + Main.rand.Next(5) * 0.1f;
            dust.fadeIn = 1.5f + Main.rand.Next(5) * 0.1f;
            dust.noGravity = true;
            dust.position = Projectile.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation - 2.1f, default) * 10f;

            dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1f);
            dust.scale = 1f + Main.rand.Next(5) * 0.1f;
            dust.noGravity = true;
            dust.position = Projectile.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation - 2.1f, default) * 10f;

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 10f)
            {
                Projectile.ai[0] = 10f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X = Projectile.velocity.X * 0.94f;

                    if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y = Projectile.velocity.Y + 0.2f;
            }
            Projectile.rotation += Projectile.velocity.X * 0.1f;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center,
                    Vector2.Zero, ModContent.ProjectileType<GrenadeExplosion>(), 30, 10f, Projectile.owner);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
        }
    }

    public class GrenadeExplosion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 60;
        }

        public override void AI()
        {
            if (Projectile.alpha == 0)
            {
                Projectile.scale = 0.1f;
            }
            Projectile.scale += 0.15f - Projectile.scale * 0.15f;
            Projectile.scale *= 1.1f;
            Projectile.alpha += 12;
            int size = (int)(Projectile.width * Projectile.scale);
            var rect = new Rectangle((int)Projectile.position.X + Projectile.width / 2 - size, (int)Projectile.position.Y + Projectile.height / 2 - size, size * 2, size * 2);
            if (Projectile.alpha < 100)
            {
                for (int i = 0; i < 16 * Projectile.scale; i++)
                {
                    var normal = Main.rand.NextVector2Unit();
                    var d = Dust.NewDustPerfect(Projectile.Center + normal * size * 0.7f * Main.rand.NextFloat(0.5f, 0.8f), DustID.Smoke, normal.RotatedBy(Main.rand.NextFloat(0.1f)) * Main.rand.NextFloat(0.5f, 1f) * 6f, Scale: Main.rand.NextFloat(0.6f, 4f) * Projectile.scale);
                    d.noGravity = true;
                }
            }
            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].CanBeChasedBy(Projectile) && Projectile.Colliding(rect, Main.npc[i].Hitbox))
                {
                    var ror = Main.npc[i].ROR();
                    ror.gasolineDamage = Math.Max(ror.gasolineDamage, Projectile.damage / 2);
                    Main.npc[i].netUpdate = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float opacity = (float)Math.Sin(Projectile.Opacity * MathHelper.Pi);
            lightColor = Color.White * opacity * 0.2f;
            lightColor.A = 0;
            Projectile.GetDrawInfo(out var t, out var off, out var frame, out var origin, out int _);

            Main.EntitySpriteDraw(t, Projectile.position + off - Main.screenPosition, frame, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(t, Projectile.position + off - Main.screenPosition, frame, lightColor * 0.8f * opacity, Projectile.rotation, origin, Projectile.scale * 1.15f, SpriteEffects.None, 0);
            return false;
        }
    }
}