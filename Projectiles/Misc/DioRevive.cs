using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace RiskOfTerrain.Projectiles.Misc
{
    public class DioRevive : ModProjectile
    {
        public override string Texture => RiskOfTerrain.AssetsPath + "LightRay3";

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if ((int)Projectile.ai[0] >= 2)
            {
                if (Projectile.timeLeft < 20)
                {
                    Projectile.alpha += 12;
                    if (Projectile.alpha > 255)
                        Projectile.alpha = 255;
                }
                else if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 20;
                    if (Projectile.alpha < 0)
                        Projectile.alpha = 0;
                }
                Projectile.velocity *= 0.90f;
                if ((int)Projectile.ai[0] == 2)
                {
                    Projectile.ai[0]++;
                }
                return;
            }
            if ((int)Projectile.ai[0] == 0)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0f, 240f), new Vector2(0f, 0f), Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 1f);
                }                
                Projectile.velocity = new Vector2(0f, -3.5f);
                Projectile.position.Y += 20f;
                Projectile.ai[0] = 2f;
                return;
            }
            if (Projectile.timeLeft < 20)
            {
                Projectile.velocity.Y -= Projectile.Opacity * 0.1f;
                Projectile.alpha += 12;
                if (Projectile.alpha > 255)
                    Projectile.alpha = 255;
            }
            else if (Projectile.alpha > 0)
            {
                Projectile.velocity.Y += 1f - Projectile.Opacity;

                Projectile.alpha -= 20;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }
            else
            {
                Projectile.velocity *= 0.8f;
            }
            Lighting.AddLight(Projectile.Center, new Vector3(0.8f, 0.8f, 0.8f) * Projectile.Opacity);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White * Projectile.Opacity;
            if ((int)Projectile.ai[0] > 2)
            {
                var font = FontAssets.MouseText.Value;
                string text = Language.GetTextValue("Mods.RiskOfTerrain.Revived");
                var measurement = font.MeasureString(text);
                lightColor *= Main.cursorAlpha;
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text, Projectile.Center - Main.screenPosition, lightColor, Color.Gray * 0.5f * Projectile.Opacity, 0f, measurement / 2f, new Vector2(Projectile.scale));

                Main.instance.LoadItem(ItemID.AngelWings);
                var item = TextureAssets.Item[ItemID.AngelWings].Value;
                var itemOrigin = item.Size() / 2f;
                Main.spriteBatch.Draw(item, Projectile.Center - new Vector2(measurement.X / 2f + item.Width / 2f, 0f) - Main.screenPosition, null, lightColor, 0f, itemOrigin, Projectile.scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(item, Projectile.Center + new Vector2(measurement.X / 2f + item.Width / 2f, 0f) - Main.screenPosition, null, lightColor, 0f, itemOrigin, Projectile.scale, SpriteEffects.FlipHorizontally, 0f);
                return false;
            }

            var texture = TextureAssets.Projectile[Type].Value;
            var frame = Projectile.Frame();
            var origin = new Vector2(frame.Width / 2f, frame.Height - 2);
            var drawCoords = new Vector2(Projectile.position.X + Projectile.width / 2f, Projectile.position.Y + Projectile.height + 10f) - Main.screenPosition;

            var scale = new Vector2(Projectile.scale * 2.5f, Projectile.scale * 2f);
            if (Projectile.timeLeft > 20)
                scale.X *= 0.2f + (float)Math.Pow(Projectile.Opacity, 3f) * 0.8f;
            scale.Y *= 0.7f + Projectile.Opacity / 2f;
            Main.EntitySpriteDraw(texture, drawCoords, frame, lightColor.UseA(0) * 0.2f, Projectile.rotation + MathHelper.Pi,
                origin, scale, SpriteEffects.None, 0);
            return false;
        }
    }
}