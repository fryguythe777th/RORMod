using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Buffs.Debuff;
using Steamworks;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

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

    public class IceSpear : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 106;
            Projectile.height = 18;
            Projectile.penetrate = 4;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.damage = 20;
            Projectile.aiStyle = -1;
            Projectile.alpha = 30;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ArtiFreeze>(), 200);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Shatter.WithPitchOffset(Main.rand.NextFloat(-0.5f, 0.6f)));

            if (!Main.dedServ)
            {
                for (int i = 0; i < Main.rand.Next(5, 11); i++)
                {
                    Dust.NewDust(Projectile.Center, 2, 2, DustID.Glass, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3), 30);
                }
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
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
            SoundEngine.PlaySound(RiskOfTerrain.GetSounds("artinanobomb/mage_m2_impact_elec_v2_", 4));
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

    public class IceWallSpawner : ModProjectile
    {
        public override string Texture => "RiskOfTerrain/Items/Accessories/T1Common/BustlingFungus";

        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.damage = 0;
            Projectile.knockBack = 0;
            Projectile.width = 1;
            Projectile.height = 1;
        }

        public int spawningStage = 1;
        public int spawnTimer = 10;

        public override void AI()
        {
            Projectile.velocity = new Vector2(0, 50);
            if (spawnTimer == 0)
            {
                spawnTimer = 10;

                if (spawningStage == 1)
                {
                    spawningStage = 2;
                    SpawnIceWall(0);
                    SoundEngine.PlaySound(RiskOfTerrain.GetSounds("artibuild/mage_shift_wall_build_ice_0", 4), Projectile.Center);
                }
                else if (spawningStage == 2)
                {
                    spawningStage = 3;
                    SpawnIceWall(30);
                    SpawnIceWall(-30);
                }
                else if (spawningStage == 3)
                {
                    SpawnIceWall(60);
                    SpawnIceWall(-60);
                    Projectile.Kill();
                }
            }
            else
            {
                spawnTimer--;
            }
        }

        public void SpawnIceWall(int xOffset)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + xOffset, Projectile.Center.Y + 42), Vector2.Zero, ModContent.ProjectileType<IceWall>(), 30, 0, Projectile.owner);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }

    public class IceWall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = 1;
            Projectile.damage = 30;
            Projectile.knockBack = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.width = 36;
            Projectile.height = 84;
            Projectile.alpha = 30;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(0, 4);
        }

        public override void AI()
        {
            if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height - 4))
            {
                Projectile.velocity = new Vector2(0, -5);
            }
            else if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height + 1))
            {
                Projectile.velocity = Vector2.Zero;
            }
            else
            {
                Projectile.velocity = new Vector2(0, 50);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.boss)
            {
                target.AddBuff(ModContent.BuffType<ArtiFreeze>(), 600);
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundStyle iceShatter = RiskOfTerrain.GetSounds("artishatter/mage_shift_wall_explode_ice_0", 4);
            iceShatter.PitchVariance = 0.5f;
            iceShatter.Pitch = 0;
            iceShatter.Volume = 0.5f;
            SoundEngine.PlaySound(iceShatter);

            if (!Main.dedServ)
            {
                for (int i = 0; i < Main.rand.Next(5, 11); i++)
                {
                    Dust.NewDust(Projectile.Center, 2, 2, DustID.Glass, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3), 30);
                }
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }
    }

    public class ElementIcon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.damage = 0;
            Projectile.knockBack = 0;
            Projectile.timeLeft = 50;
        }

        Vector2 presentOffset;
        Vector2 targetOffset;

        public override void OnSpawn(IEntitySource source)
        {
            string position = "center";
            if (Projectile.ai[0] == 1) //is a fireball
            {
                Projectile.frame = 0;

                if (Projectile.ai[1] == 1) //fireball is being selected
                {
                    position = "right";
                }
                else if (Projectile.ai[1] == 2) //plasma is being selected
                {
                    position = "center";
                }
                else if (Projectile.ai[1] == 3) //ice is being selected
                {
                    position = "left";
                }
            }
            else if (Projectile.ai[0] == 2) //is a plasma
            {
                Projectile.frame = 1;

                if (Projectile.ai[1] == 1) //fireball is being selected
                {
                    position = "left";
                }
                else if (Projectile.ai[1] == 2) //plasma is being selected
                {
                    position = "right";
                }
                else if (Projectile.ai[1] == 3) //ice is being selected
                {
                    position = "center";
                }
            }
            else //is ice
            {
                Projectile.frame = 2;

                if (Projectile.ai[1] == 1) //fireball is being selected
                {
                    position = "center";
                }
                else if (Projectile.ai[1] == 2) //plasma is being selected
                {
                    position = "left";
                }
                else if (Projectile.ai[1] == 3) //ice is being selected
                {
                    position = "right";
                }
            }

            switch (position)
            {
                case "left":
                    presentOffset = new Vector2(40, -40);
                    Projectile.Center = Main.player[Projectile.owner].Center + presentOffset;
                    targetOffset = new Vector2(20, -40);
                    break;
                case "center":
                    presentOffset = new Vector2(-0, -40);
                    Projectile.Center = Main.player[Projectile.owner].Center + presentOffset;
                    targetOffset = new Vector2(-20, -40);
                    break;
                case "right":
                    presentOffset = new Vector2(20, -40);
                    Projectile.Center = Main.player[Projectile.owner].Center + presentOffset;
                    targetOffset = new Vector2(0, -40);
                    break;
            }
        }

        public override void AI()
        {
            Projectile.alpha = (int)Math.Abs(Projectile.Center.X - Main.player[Projectile.owner].Center.X) * 10;
            Projectile.scale = (20 - Math.Abs(Projectile.Center.X - Main.player[Projectile.owner].Center.X) + 1) / 20;

            presentOffset = Vector2.Lerp(presentOffset, targetOffset, 0.06f);
            Projectile.Center = Main.player[Projectile.owner].Center + presentOffset;

            if (Math.Abs(Main.player[Projectile.owner].Center.X + presentOffset.X - Main.player[Projectile.owner].Center.X + targetOffset.X) < 1f)
            {
                Projectile.Kill();
            }
        }
    }
}