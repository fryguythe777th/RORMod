using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Buffs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Misc
{
    public class BustlingFungusProj : ModProjectile
    {
        public List<Vector3> fungusInfo;
        public bool playedSound;
        public bool accessoryActive;
        public float regenPercent;

        protected override bool CloneNewInstances => base.CloneNewInstances;

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.hide = true;
            Projectile.timeLeft = 20;
            fungusInfo = new List<Vector3>();
            playedSound = false;
        }

        public override void AI()
        {
            if (!playedSound)
            {
                SoundEngine.PlaySound(RiskOfTerrain.GetSound("bungus", volume: 0.3f), Projectile.Center);
                playedSound = true;
            }
            float scale = Projectile.scale;
            Projectile.Center = Main.player[Projectile.owner].Center;
            var ror = Main.player[Projectile.owner].ROR();
            bool active = true;
            if (Projectile.numUpdates == -1)
            {
                active = accessoryActive;
                accessoryActive = false;
                //Main.NewText(active + ": " + Projectile.numUpdates + ", " + Projectile.whoAmI);
            }
            if (Main.netMode != NetmodeID.Server && Projectile.numUpdates == -1)
            {
                ManageBungalTendices(Main.player[Projectile.owner], ror, active);
            }

            if (active)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active && !Main.player[i].dead && Main.player[i].Distance(Projectile.Center) < Projectile.scale / 2f &&
                        (!Main.player[i].hostile || !Main.player[Projectile.owner].hostile || Main.player[i].team == Main.player[Projectile.owner].team))
                    {
                        Main.player[i].ROR().increasedRegen += Math.Clamp((int)(Main.player[i].statLifeMax2 * regenPercent), 1, 20);
                        Main.player[i].AddBuff(ModContent.BuffType<BustlingFungusBuff>(), 4, quiet: true);
                    }
                }
            }
            else
            {
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 0.2f);
            }

            if (Projectile.scale > 0.1f)
            {
                Projectile.timeLeft = 2;
            }
            if (scale != Projectile.scale)
            {
                Projectile.netUpdate = true;
            }

            if (Projectile.scale > 0.1f)
            {
                foreach (var c in Helpers.CircularVector((int)(32 * Projectile.scale)))
                {
                    Lighting.AddLight(Projectile.Center + c * Projectile.scale / 2f, new Vector3(0.1f, 1f, 0.2f) * Projectile.scale / 400f * 0.5f);
                }
            }

            Lighting.AddLight(Projectile.Center, new Vector3(0.1f, 1f, 0.2f) * Projectile.scale / 280f);
        }
        public void ManageBungalTendices(Player player, RORPlayer ror, bool active)
        {
            if (fungusInfo == null)
                fungusInfo = new List<Vector3>();

            if (active && Main.GameUpdateCount % 10 == 0)
            {
                bool spawnFungus = Main.rand.NextBool(1 + fungusInfo.Count * 2);
                int xStart = (int)(Projectile.position.X - Projectile.scale / 2f) / 16;
                int xEnd = (int)(Projectile.position.X + Projectile.scale / 2f) / 16;
                int yStart = (int)(Projectile.position.Y - Projectile.scale / 2f) / 16;
                int yEnd = (int)(Projectile.position.Y + Projectile.scale / 2f) / 16;
                var validSpots = new List<Point>();
                for (int i = xStart; i <= xEnd; i++)
                {
                    for (int j = yStart; j <= yEnd; j++)
                    {
                        if (WorldGen.InWorld(i, j, 5))
                        {
                            if ((!Main.tile[i, j].HasTile || !Main.tileSolid[Main.tile[i, j].TileType]) && Main.tile[i, j + 1].HasTile && Main.tileSolid[Main.tile[i, j + 1].TileType])
                            {
                                validSpots.Add(new Point(i, j));
                            }
                        }
                    }
                }

                if (validSpots.Count > 0)
                {
                    while (validSpots.Count > 0)
                    {
                        var rand = Main.rand.Next(validSpots);
                        float randX = Main.rand.NextFloat(16f);
                        var endLoc = new Vector2(rand.X * 16f + randX, rand.Y * 16f + Main.rand.NextFloat(-2f, 6f) + 12f);
                        if (Main.tile[rand.X, rand.Y + 1].IsHalfBlock)
                        {
                            endLoc.X += 8f;
                        }
                        if (Main.tile[rand.X, rand.Y + 1].Slope == SlopeType.SlopeDownRight)
                        {
                            endLoc.Y += 16f - randX;
                        }
                        if (Main.tile[rand.X, rand.Y + 1].Slope == SlopeType.SlopeDownLeft)
                        {
                            endLoc.Y += randX;
                        }
                        var d = Dust.NewDustPerfect(endLoc, DustID.TintableDustLighted, Velocity: new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-1f, -0.2f)), newColor: new Color(100, 255, 160, 80));
                        d.scale *= 0.4f;
                        d.fadeIn = d.scale / 0.4f;
                        if (ror.cBungus != 0)
                        {
                            d.shader = GameShaders.Armor.GetSecondaryShader(ror.cBungus, player);
                        }
                        float diff = d.position.X - Projectile.Center.X;
                        bool end = true;
                        if (Math.Abs(diff) > Projectile.scale / 2f - 16f)
                        {
                            if (Math.Sign(d.velocity.X) != Math.Sign(-diff))
                            {
                                d.velocity.X = -d.velocity.X;
                            }
                            spawnFungus = false;
                        }
                        for (int i = 0; i < fungusInfo.Count; i++)
                        {
                            if ((endLoc - new Vector2(fungusInfo[i].X, fungusInfo[i].Y)).Length() < 14f)
                            {
                                end = false;
                                continue;
                            }
                        }
                        if (end)
                        {
                            if (spawnFungus)
                                fungusInfo.Add(new Vector3(endLoc.X, endLoc.Y, 0f));
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < fungusInfo.Count; i++)
            {
                float nextZ = fungusInfo[i].Z + 1f;
                if (!active && nextZ < 80f)
                {
                    nextZ = 80f;
                }
                fungusInfo[i] = new Vector3(fungusInfo[i].X, fungusInfo[i].Y, nextZ);
                if (fungusInfo[i].Z > 120f)
                {
                    fungusInfo.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.PrepareDrawnEntityDrawing(Projectile, Main.player[Projectile.owner].ROR().cBungus);
            DrawAura(Projectile.Center - Main.screenPosition, Projectile.scale, Projectile.Opacity, ModContent.Request<Texture2D>(Texture + "Aura").Value, TextureAssets.Projectile[Type].Value);
            var fungus = ModContent.Request<Texture2D>($"{Texture}Fungus", AssetRequestMode.ImmediateLoad).Value;
            foreach (var f in fungusInfo)
            {
                float time = f.Z;
                float x = f.X;
                float y = f.Y;
                float opacity = 1f;
                var scale = new Vector2(1f);
                float goUp = 20f;
                if (time > 120f - goUp * 2f)
                {
                    time = (240f - time * 2f) / 2f;
                }
                if (time < goUp)
                {
                    y += (float)Math.Pow(1f - time / goUp, 2f) * goUp;
                    opacity = time / 20f;

                    scale.X *= Math.Max((float)Math.Pow(time / goUp, 2f), 0.4f);
                }
                scale *= 1f + (float)(Math.Sin(x) * 0.2f);

                Main.EntitySpriteDraw(fungus, new Vector2(x, y) - Main.screenPosition, null,
                    Color.White * Projectile.Opacity * opacity, (float)Math.Sin(x + f.Z * 0.01f) * 0.2f, new Vector2(fungus.Width / 2f, fungus.Height - 6f), scale, SpriteEffects.None, 0);
            }
            return false;
        }

        public static void DrawAura(Vector2 location, float diameter, float opacity, Texture2D texture, Texture2D circle)
        {
            var origin = texture.Size() / 2f;
            location = location.Floor();
            float scale = diameter / texture.Width;
            opacity = Math.Min(opacity * scale, 1f);

            var color = new Color(255, 255, 255, 0);
            Main.EntitySpriteDraw(texture, location, null,
                color * 0.3f * opacity, 0f, origin, scale, SpriteEffects.None, 0);

            foreach (var c in Helpers.CircularVector(4))
            {
                Main.EntitySpriteDraw(circle, location + c, null,
                    Color.White * opacity, 0f, origin, scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(circle, location, null,
                Color.White * opacity, 0f, origin, scale, SpriteEffects.None, 0);
        }
    }
}