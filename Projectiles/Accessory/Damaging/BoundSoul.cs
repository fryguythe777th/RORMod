using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Graphics.Primitives;
using RiskOfTerrain.Projectiles.Elite;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
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
        public Vector2 blazeSpotPrev;

        public override void AI()
        {
            //code to calculate this soul's orbital position
            Player player = Main.player[Projectile.owner];
            Vector2 offset = new Vector2(0, 100).RotatedBy((MathHelper.TwoPi / player.ROR().boundSoulCount) * Projectile.ai[0]);
            Vector2 intendedPos = player.Center + offset.RotatedBy(MathHelper.ToRadians(player.ROR().boundSoulRotTick));

            if (Projectile.ai[1] == 1f) //boss soul size diff
            {
                Projectile.width = 27;
                Projectile.height = 27;
                Projectile.scale = 1.5f;
            }

            if (state == 1) //first ai state, trying to reach orbital position
            {
                if (player.ROR().releaseTheGhosts) //go to state 3 if the player takes dmg
                {
                    state = 3;
                }

                if (state == 1) //else proceed
                {
                    //code to make him go towards orbital position w/ easing
                    Projectile.timeLeft = 600;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, (intendedPos - Projectile.Center).SafeNormalize(-Vector2.UnitY) * 12f, 0.08f);
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

                    //enter state 2 if orbital position is reached
                    if (Math.Abs(Projectile.Center.X - intendedPos.X) < 1 && Math.Abs(Projectile.Center.Y - intendedPos.Y) < 1)
                    {
                        state = 2;
                        savedSoulCount = player.ROR().boundSoulCount;
                    }
                }

                if (Projectile.ai[2] == 2) //celestine passive effect
                {
                    player.AddBuff(BuffID.Invisibility, 2);
                }
                else if (Projectile.ai[2] == 5) //mending passive effect
                {
                    player.lifeRegen += 10;
                }
            }

            if (state == 2) //second ai state, maintaining orbital position
            {
                if (player.ROR().releaseTheGhosts) //go to state 3 if the player takes dmg
                {
                    state = 3;
                }

                if (state == 2) //else proceed
                {
                    if (player.ROR().boundSoulCount > savedSoulCount) //go back to state 1 if the circle is resized
                    {
                        state = 1;
                    }

                    savedSoulCount = player.ROR().boundSoulCount;

                    if (state == 2) //else maintain orbital position
                    {
                        Projectile.timeLeft = 600;
                        Projectile.Center = intendedPos;
                        Projectile.rotation = MathHelper.Lerp(Projectile.rotation, 0, 0.08f);
                    }
                }

                if (Projectile.ai[2] == 2) //celestine passive effect
                {
                    player.AddBuff(BuffID.Invisibility, 2);
                }
                else if (Projectile.ai[2] == 5) //mending passive effect
                {
                    player.lifeRegen += 10;
                }
            }

            if (state == 3) //third ai state, seeking enemies
            {
                //make him dangerous
                Projectile.damage = 30;
                Projectile.knockBack = 4;
                Projectile.penetrate = 1;

                if (Projectile.ai[1] == 1f) //boss soul dmg, kb, penetrate boost
                {
                    Projectile.damage *= 2;
                    Projectile.knockBack *= 2;
                    Projectile.penetrate *= 5;
                }

                if (Projectile.ai[2] == 6) //overloading penetrate boost
                {
                    Projectile.penetrate++;
                }

                Projectile.friendly = true;
                Projectile.hostile = false;

                //seeking code
                int closest = Projectile.FindTargetWithLineOfSight();

                if (closest != -1 && Main.npc[closest].active && Main.npc[closest] != null && Main.npc[closest].life > 0)
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, (Main.npc[closest].Center - Projectile.Center).SafeNormalize(-Vector2.UnitY) * 12f, 0.08f);
                }

                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

                Projectile.velocity *= 1.05f;

                if (Projectile.ai[2] == 1) //blazing trail effect
                {
                    if (blazeSpotPrev == Vector2.Zero)
                        blazeSpotPrev = Projectile.position;

                    var diff = Projectile.position - blazeSpotPrev;
                    float distance = diff.Length().UnNaN();
                    if (Main.netMode != NetmodeID.MultiplayerClient && (Main.GameUpdateCount % 40 == 0 || distance > 40f))
                    {
                        var v = new Vector2(Projectile.position.X + Projectile.width / 2f, Projectile.position.Y + Projectile.height - 10f);
                        for (int i = 0; i <= (int)(distance / 100f); i++)
                        {
                            var p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), v + Vector2.Normalize(-diff).UnNaN() * 50f * i,
                                Main.rand.NextVector2Unit() * 0.2f, ProjectileID.GreekFire1 + Main.rand.Next(3), 10, 1f, Projectile.owner, 1f, 1f);

                            p.timeLeft /= 3;
                            p.ROR().spawnedFromElite = true;
                            p.hostile = false;
                            p.friendly = true;
                        }
                        blazeSpotPrev = Projectile.position;
                    }
                }
            }
        }

        public static ArmorShaderData shader;

        public static bool DrawingElite;

        public override bool PreDraw(ref Color lightColor) //make him shaded if hes an elite
        {
            switch (Projectile.ai[2])
            {
                case 0:
                    shader = null;
                    break;
                case 1:
                    shader = GameShaders.Armor.GetShaderFromItemId(ItemID.RedDye);
                    break;
                case 2:
                    shader = GameShaders.Armor.GetShaderFromItemId(ItemID.FogboundDye);
                    break;
                case 3:
                    shader = GameShaders.Armor.GetShaderFromItemId(ItemID.SilverDye);
                    break;
                case 4:
                    shader = GameShaders.Armor.GetShaderFromItemId(ItemID.GreenandBlackDye);
                    break;
                case 5:
                    shader = GameShaders.Armor.GetShaderFromItemId(ItemID.GreenDye);
                    break;
                case 6:
                    shader = GameShaders.Armor.GetShaderFromItemId(ItemID.BlueDye);
                    break;
                case 7:
                    shader = GameShaders.Armor.GetShaderFromItemId(ItemID.BlueAcidDye);
                    break;
                case 8:
                    shader = GameShaders.Armor.GetShaderFromItemId(ItemID.PurpleDye);
                    break;
            }

            if (shader != null)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.Transform);
                shader.Apply(Projectile);
                DrawingElite = true;
            } 

            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            if (DrawingElite)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.Transform);
                DrawingElite = false;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[2] == 2) //celestine active effect
            {
                target.AddBuff(ModContent.BuffType<CelestineSlow>(), 240);
            }
            else if (Projectile.ai[2] == 3) //glacial active effect
            {
                target.AddBuff(ModContent.BuffType<RunaldFreeze>(), 240);
            }
            else if (Projectile.ai[2] == 5) //mending active effect
            {
                Main.player[Projectile.owner].Heal(damageDone);
            }
            else if (Projectile.ai[2] == 6) //overloading active effect
            {
                int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center + new Vector2(Main.rand.Next(0, 16), Main.rand.Next(0, 16)), Vector2.Zero, ModContent.ProjectileType<OverloadingBomb>(), 0, 0, Owner: Projectile.owner, ai2: target.whoAmI, ai1: 2);
                Main.projectile[p].ROR().spawnedFromElite = true;
                Main.projectile[p].friendly = true;
                Main.projectile[p].hostile = false;
                Main.projectile[p].ai[1] = 2;
            }
        }

        public override void Kill(int timeLeft) //death spiciness
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