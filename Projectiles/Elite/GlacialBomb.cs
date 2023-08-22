using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Projectiles.Accessory.Utility;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Elite
{
    public class GlacialBomb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.hide = true;
            Projectile.timeLeft = 180;
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 velocity = new Vector2(0, 3).RotatedBy(MathHelper.ToRadians(36 * i));
                Dust.NewDust(Projectile.Center, 2, 2, DustID.Clentaminator_Blue, velocity.X, velocity.Y);
            }
            SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);
        }

        public override void AI()
        {
            Projectile.scale = MathHelper.Lerp(Projectile.scale, 400f, 0.2f);

            if (Projectile.scale > 0.1f)
            {
                foreach (var c in Helpers.CircularVector((int)(64 * Projectile.scale)))
                {
                    Lighting.AddLight(Projectile.Center + c * Projectile.scale / 2f, new Vector3(0.1f, 1f, 0.2f) * Projectile.scale / 400f * 0.33f);
                }
            }

            if (400f - Projectile.scale < 20f)
            {
                Projectile.Kill();
            }

            Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.95f, 0.85f) * Projectile.scale / 280f);
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[0] == 0)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active && !Main.player[i].dead && Main.player[i].Distance(Projectile.Center) < Projectile.scale / 2f)
                    {
                        Main.player[i].Hurt(PlayerDeathReason.ByNPC(Projectile.owner), 40, 0);
                        Main.player[i].AddBuff(BuffID.Frozen, 300);
                    }
                }

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && npc.Distance(Projectile.Center) < Projectile.scale / 2f && (npc.friendly || npc.lifeMax <= 5 || npc.damage == 0))
                    {
                        Projectile.friendly = false;
                        Projectile.hostile = true;
                        Projectile.damage = 40;
                        npc.AddBuff(ModContent.BuffType<RunaldFreeze>(), 600);
                    }
                }
            }
            else if (Projectile.ai[0] == 1)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && npc.Distance(Projectile.Center) < Projectile.scale / 2f && !npc.friendly && npc.lifeMax > 5 && npc.damage > 0)
                    {
                        NPC.HitInfo hit = new NPC.HitInfo();
                        hit.Crit = false;
                        hit.SourceDamage = 40;
                        hit.DamageType = DamageClass.Generic;
                        hit.Knockback = 0;

                        npc.StrikeNPC(hit);
                        NetMessage.SendStrikeNPC(npc, hit);
                        npc.AddBuff(ModContent.BuffType<RunaldFreeze>(), 600);
                    }
                }
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            BustlingFungusProj.DrawAura(Projectile.Center - Main.screenPosition, Projectile.scale, Projectile.Opacity * 0.4f, ModContent.Request<Texture2D>(Texture + "Aura").Value, TextureAssets.Projectile[Type].Value);
            return false;
        }
    }
}