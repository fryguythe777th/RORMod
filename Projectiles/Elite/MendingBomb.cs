using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Buffs;
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
    public class MendingBomb : ModProjectile
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
                Dust.NewDust(Projectile.Center, 2, 2, DustID.Clentaminator_Green, velocity.X, velocity.Y);
            }
            SoundEngine.PlaySound(RiskOfTerrain.GetSound("bungus", volume: 0.3f), Projectile.Center);
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
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead && Main.player[i].Distance(Projectile.Center) < Projectile.scale / 2f && Main.player[i].statLife < Main.player[i].statLifeMax2)
                {
                    Main.player[i].statLife += Math.Clamp((int)(Main.player[i].statLifeMax2 * 0.15), 1, Main.player[i].statLifeMax2 - Main.player[i].statLife);
                }
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.Distance(Projectile.Center) < Projectile.scale / 2f && npc.life < npc.lifeMax)
                {
                    npc.life += Math.Clamp((int)(npc.lifeMax * 0.15), 1, npc.lifeMax - npc.life);
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