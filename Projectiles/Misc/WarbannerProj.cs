using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RORMod.Buffs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Projectiles.Misc
{
    public class WarbannerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 48;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 100;
            Projectile.hide = true;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.3f;
            if ((int)Projectile.ai[0] == 0)
            {
                Projectile.scale = 0.1f;
                Projectile.rotation = Main.rand.NextFloat(-0.2f, 0.2f);
                Projectile.spriteDirection = Main.player[Projectile.owner].direction;
                Projectile.ai[0] = 1f;
            }

            if (Projectile.extraUpdates == 0 && Projectile.numUpdates == -1)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active && !Main.player[i].dead && Projectile.Distance(Main.player[i].Center) < 642f)
                    {
                        Main.player[i].AddBuff(ModContent.BuffType<WarbannerBuff>(), 4, quiet: Main.player[i].HasBuff<WarbannerBuff>());
                    }
                }

                if (Projectile.scale > 0.66f)
                {
                    for (int i = 0; i < 40 * Projectile.scale; i++)
                    {
                        var dustLoc = Projectile.Center + Main.rand.NextVector2Unit() * (Main.rand.NextFloat(80f, 650f) * Projectile.scale);
                        if (Lighting.GetColor(dustLoc.ToTileCoordinates()).ToVector3().Length() < 0.5f)
                        {
                            float distance = Projectile.Distance(dustLoc);
                            if (distance < 500f)
                            {
                                int chance = 65 - (int)distance / 10;
                                if (Main.rand.Next(chance) < chance - 1)
                                {
                                    continue;
                                }
                            }
                            var d = Dust.NewDustPerfect(dustLoc, DustID.YellowTorch);
                            d.velocity *= 2f;
                            d.velocity += Projectile.DirectionFrom(dustLoc);
                            d.noGravity = true;
                            d.scale *= distance / 650f;
                            d.fadeIn = d.scale + 1f;
                        }
                    }
                }

                if ((int)Projectile.ai[0] == 2)
                {
                    Projectile.scale -= 0.01f;
                    if (Projectile.scale < 1f)
                    {
                        Projectile.scale = 1f;
                    }
                }
                else
                {
                    Projectile.scale += 0.1f;
                    if (Projectile.scale > 1.1f)
                    {
                        Projectile.ai[0] = 2f;
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.extraUpdates = 0;
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = TextureAssets.Projectile[Type].Value;
            Main.EntitySpriteDraw(texture, new Vector2(Projectile.position.X + Projectile.width / 2f, Projectile.position.Y + Projectile.height) - Main.screenPosition, null, lightColor, Projectile.rotation,
                new Vector2(texture.Width / 2f, texture.Height - 2), Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            var circle = ModContent.Request<Texture2D>(Texture + "Circle", AssetRequestMode.ImmediateLoad);
            var circleOrigin = circle.Size() / 2f;
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture + "Aura", AssetRequestMode.ImmediateLoad).Value, Projectile.Center - Main.screenPosition, null, Color.White * 0.4f, 0f,
                circleOrigin, Math.Min(Projectile.scale, 1f) * 2f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(circle.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), 0f,
                circleOrigin, Math.Min(Projectile.scale, 1f) * 2f, SpriteEffects.None, 0);
            return false;
        }
    }
}