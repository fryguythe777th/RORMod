using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Buffs.Debuff;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Artificer
{
    public class PlasmaBoltImpact : ModProjectile
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
                    var d = Dust.NewDustPerfect(Projectile.Center + normal * size * 0.7f * Main.rand.NextFloat(0.5f, 0.8f), DustID.BlueTorch, normal.RotatedBy(Main.rand.NextFloat(0.1f)) * Main.rand.NextFloat(0.5f, 1f) * 6f, Scale: Main.rand.NextFloat(0.6f, 4f) * Projectile.scale);
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] == 1)
            {
                target.AddBuff(ModContent.BuffType<StunDebuff>(), 180);
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

    public class PlasmaBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 10;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 22;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.damage = 10;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (++Projectile.frameCounter >= 5)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.BlueFlare, Vector2.Zero);
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            Lighting.AddLight(Projectile.Center, TorchID.Blue);
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center,
                    Vector2.Zero, ModContent.ProjectileType<PlasmaBoltImpact>(), 10, 3f, Projectile.owner);
            SoundStyle pvz = new SoundStyle();
            pvz.SoundPath = "RiskOfTerrain/Assets/Sounds/firepea";
            pvz.PitchVariance = 0.25f;
            pvz.Pitch = -3f;
            pvz.Volume = 5f;
            SoundEngine.PlaySound(pvz);
        }
    }

    public class Nanobomb : ModProjectile
    {
        public override string Texture => "RiskOfTerrain/Items/CharacterSets/Artificer/PlasmaBolt";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 10;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 22;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.damage = 10;
            Projectile.aiStyle = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = 1.5f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (++Projectile.frameCounter >= 5)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.BlueFlare, Vector2.Zero);
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            Lighting.AddLight(Projectile.Center, TorchID.Blue);
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center,
                    Vector2.Zero, ModContent.ProjectileType<PlasmaBoltImpact>(), 40, 3f, Projectile.owner, 1);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<IonSurgeVisual>(), 0, 0, Projectile.owner, 1);
            SoundStyle pvz = new SoundStyle();
            pvz.SoundPath = "RiskOfTerrain/Assets/Sounds/firepea";
            pvz.PitchVariance = 0.25f;
            pvz.Pitch = -3f;
            pvz.Volume = 5f;
            SoundEngine.PlaySound(pvz);
        }
    }

    public class FireBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 18;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.damage = 10;
            Projectile.aiStyle = -1;
            Projectile.alpha = 50;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (++Projectile.frameCounter >= 5)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Flare, Vector2.Zero);
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            Lighting.AddLight(Projectile.Center, TorchID.Torch);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }

        public override void Kill(int timeLeft)
        {
            SoundStyle pvz = new SoundStyle();
            pvz.SoundPath = "RiskOfTerrain/Assets/Sounds/firepea";
            pvz.PitchVariance = 0.25f;
            pvz.Volume = 5f;
            SoundEngine.PlaySound(pvz);
            for (int i = 0; i < Main.rand.Next(3, 8); i++)
            {
                Dust.NewDust(Projectile.Center, 1, 1, DustID.Flare, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
            }
        }
    }

    public class IonSurgeVisual : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 10;
        }

        public override void SetDefaults()
        {
            Projectile.width = 268;
            Projectile.height = 268;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.damage = 0;
            Projectile.knockBack = 0;
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.Kill();
            }
        }
    }
}