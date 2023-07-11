using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Graphics.Primitives;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Damaging
{
    public class BoundSoul : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.damage = 0;
            Projectile.knockBack = 0;
            Projectile.penetrate = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(0, 4);
        }

        public int state = 1;
        public int savedSoulCount;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 offset = new Vector2(0, 100).RotatedBy((MathHelper.TwoPi / player.ROR().boundSoulCount) * Projectile.ai[0]);
            Vector2 intendedPos = player.Center + offset.RotatedBy(MathHelper.ToRadians(player.ROR().boundSoulRotTick));

            if (state == 1)
            {
                if (player.ROR().releaseTheGhosts)
                {
                    state = 3;
                }

                if (state == 1)
                {
                    Projectile.timeLeft = 600;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, (intendedPos - Projectile.Center).SafeNormalize(-Vector2.UnitY) * 12f, 0.08f);
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

                    if (Math.Abs(Projectile.Center.X - intendedPos.X) < 1 && Math.Abs(Projectile.Center.Y - intendedPos.Y) < 1)
                    {
                        state = 2;
                        savedSoulCount = player.ROR().boundSoulCount;
                    }
                }
            }

            if (state == 2)
            {
                if (player.ROR().releaseTheGhosts)
                {
                    state = 3;
                }

                if (state == 2)
                {
                    if (player.ROR().boundSoulCount > savedSoulCount)
                    {
                        state = 1;
                    }

                    savedSoulCount = player.ROR().boundSoulCount;

                    if (state == 2)
                    {
                        Projectile.timeLeft = 600;
                        Projectile.Center = intendedPos;
                        Projectile.rotation = MathHelper.Lerp(Projectile.rotation, 0, 0.08f);
                    }
                }
            }

            if (state == 3)
            {
                Projectile.damage = 30;
                Projectile.knockBack = 4;
                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.penetrate = 1;

                int closest = Projectile.FindTargetWithLineOfSight();

                if (closest != -1 && Main.npc[closest].active && Main.npc[closest] != null && Main.npc[closest].life > 0)
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, (Main.npc[closest].Center - Projectile.Center).SafeNormalize(-Vector2.UnitY) * 12f, 0.08f);
                }

                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

                Projectile.velocity *= 1.05f;
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath6);

            for (int i = 0; i < 3; i++)
            {
                int size = Main.rand.Next(1, 3);
                Dust.NewDust(Projectile.Center, size, size, DustID.Ghost, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3), 150);
            }
        }
    }
}