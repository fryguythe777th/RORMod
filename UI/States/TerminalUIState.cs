using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RORMod.Items;
using RORMod.Tiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace RORMod.UI.States
{
    public class TerminalUIState : RORUIState
    {
        public static Dictionary<int, Func<string>> DynamicTooltip { get; private set; }
        public static Dictionary<int, Func<string>> DynamicLog { get; private set; }

        public int selectedItem;
        public Item hoverItem;

        public override void Load(Mod mod)
        {
            DynamicTooltip = new Dictionary<int, Func<string>>();
            DynamicLog = new Dictionary<int, Func<string>>();
        }

        public override void Unload()
        {
            DynamicTooltip?.Clear();
            DynamicTooltip = null;
            DynamicLog?.Clear();
            DynamicLog = null;
        }

        public override void OnInitialize()
        {
            OverrideSamplerState = SamplerState.LinearClamp;

            Width.Set(0, 0.5f);
            Height.Set(0, 0.526f);
            MinWidth.Set(700, 0f);
            Top.Set(0, 0.75f - Height.Percent);
            HAlign = 0.5f;

            Main.playerInventory = false;
            SoundEngine.PlaySound(SoundID.MenuOpen);
        }

        public override void OnDeactivate()
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Main.playerInventory || (Main.LocalPlayer.controlInv && Main.LocalPlayer.releaseInventory) || !Main.LocalPlayer.adjTile[ModContent.TileType<TerminalTile>()])
            {
                RORUI.DynamicInterface.SetState(null);
                SoundEngine.PlaySound(SoundID.MenuClose);
            }
            Main.LocalPlayer.AdjTiles();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var d = GetDimensions();
            var rect = d.ToRectangle();
            if (rect.Contains(Main.mouseX, Main.mouseY))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            Helpers.DrawRectangle(d.ToRectangle(), new Color(15, 15, 40, 255));

            var texture = ModContent.Request<Texture2D>($"{RORMod.AssetsPath}UI/Terminal", AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(d.X, d.Y), new Rectangle(27, 1, 1, 11), Color.White, 0f, Vector2.Zero, new Vector2(d.Width, 2f), SpriteEffects.None, 0f);

            spriteBatch.Draw(texture, new Vector2(d.X, d.Y), new Rectangle(2, 1, 23, 11), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, new Vector2(d.X + d.Width - 60, d.Y), new Rectangle(30, 1, 30, 11), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            Helpers.DrawRectangle(new Rectangle((int)d.X + 12, (int)d.Y + 27, (int)d.Width - 24, 24), new Color(30, 30, 60, 255));
            DrawHomePage(spriteBatch, texture, d, rect);
            DrawDeliveryTab(spriteBatch, texture, d, rect);
            base.DrawSelf(spriteBatch);
        }

        public void DrawHomePage(SpriteBatch spriteBatch, Texture2D texture, CalculatedStyle d, Rectangle rect)
        {
            spriteBatch.Draw(texture, new Vector2(d.X + 12, d.Y + 42 + 14), new Rectangle(367, 399, 77, 19), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, new Vector2(d.X + 12, d.Y + 42 * 2 + 14), new Rectangle(452, 399, 77, 19), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, new Vector2(d.X + 12, d.Y + 42 * 3 + 14), new Rectangle(367, 426, 77, 19), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
        }

        public void DrawDeliveryTab(SpriteBatch spriteBatch, Texture2D texture, CalculatedStyle d, Rectangle rect)
        {
            var clr = new Color(155, 155, 180, 255);
            var clr2 = new Color(72, 72, 100, 255);
            Helpers.DrawRectangle(new Rectangle((int)d.X + 200, (int)d.Y + 42 + 14, (int)d.Width - 212, (int)d.Height - 62), clr);
            Helpers.DrawRectangle(new Rectangle((int)d.X + 204, (int)d.Y + 42 + 14 + 4, (int)d.Width - 220, (int)d.Height - 62 - 8), clr2);

            int x = 0;
            int y = 0;
            int width = 44;
            int height = 44;
            int spriteSize = 36;
            int maxWidth = (int)Math.Max((d.Width - 212) / 2, width) / width;

            int iconInfoTabX = (int)d.X + 208 + maxWidth * width + width;
            int iconInfoTabY = (int)d.Y + 42 + 14 + 4;
            int iconInfoTabWidth = (int)d.Width - 224 - maxWidth * width - width;
            Helpers.DrawRectangle(new Rectangle(iconInfoTabX, iconInfoTabY, iconInfoTabWidth, (int)d.Height - 62 - 8), clr);
            Helpers.DrawRectangle(new Rectangle(iconInfoTabX + 4, iconInfoTabY, 10, (int)d.Height - 62 - 8), clr2);

            DrawDeliveryTab_DrawItems(spriteBatch, (int)d.X, (int)d.Y, ref x, ref y, width, height, spriteSize, maxWidth, clr, new Color(100, 100, 110, 255), RORItem.WhiteTier);
            DrawDeliveryTab_DrawItems(spriteBatch, (int)d.X, (int)d.Y, ref x, ref y, width, height, spriteSize, maxWidth, new Color(15, 100, 15, 255) * 2, new Color(66, 90, 60, 255), RORItem.GreenTier);
            DrawDeliveryTab_DrawItems(spriteBatch, (int)d.X, (int)d.Y, ref x, ref y, width, height, spriteSize, maxWidth, new Color(100, 15, 15, 255) * 2, new Color(90, 60, 60, 255), RORItem.RedTier);

            if (selectedItem > 0)
            {
                string text = Lang.GetItemNameValue(selectedItem);
                iconInfoTabX += 12;
                iconInfoTabWidth -= 12;
                var font = FontAssets.MouseText.Value;
                var textOrigin = new Vector2(font.MeasureString(text).X / 2f, 0f);
                var t = ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, text, new Vector2(iconInfoTabX + iconInfoTabWidth / 2f, iconInfoTabY),
                    Color.White, new Color(35, 35, 66), 0f, textOrigin, Vector2.One, iconInfoTabWidth);
                int textHeight = (int)(t.Y - iconInfoTabY);
                Helpers.DrawRectangle(new Rectangle(iconInfoTabX + 6, iconInfoTabY + textHeight + 28, iconInfoTabWidth - 4, (int)d.Height - 62 - 8 - textHeight - 28), clr2);

                text = "";
                if (DynamicTooltip.TryGetValue(selectedItem, out var dynamicTooltip))
                {
                    text = dynamicTooltip();
                }
                else if (selectedItem >= Main.maxItemTypes)
                {
                    var modItem = ItemLoader.GetItem(selectedItem);
                    string detailedTTKey = $"Mods.{modItem.Mod.Name}.Terminal.{modItem.Name}.Tooltip";
                    string detailedTT = Language.GetTextValue(detailedTTKey);
                    if (detailedTT != detailedTTKey)
                    {
                        text = detailedTT;
                    }
                }
                if (text == "")
                {
                    text = GetItemTooltipAsOneLine(selectedItem);
                }

                t = ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, GetColorCodedString(text), new Vector2(iconInfoTabX + 12f, iconInfoTabY + 32f),
                    Color.White, new Color(35, 35, 66), 0f, Vector2.Zero, Vector2.One * 0.75f, iconInfoTabWidth - 12);

                int descHeight = (int)(t.Y - iconInfoTabY);
                Helpers.DrawRectangle(new Rectangle(iconInfoTabX + 6, iconInfoTabY + textHeight + descHeight + 22, iconInfoTabWidth - 4, 4), clr);

                if (DynamicLog.TryGetValue(selectedItem, out var dynamicLog))
                {
                    text = dynamicLog();
                }
                else if (selectedItem >= Main.maxItemTypes)
                {
                    var modItem = ItemLoader.GetItem(selectedItem);
                    text = Language.GetTextValue($"Mods.{modItem.Mod.Name}.Terminal.{modItem.Name}.Lore");
                }

                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, text, new Vector2(iconInfoTabX + 12f, iconInfoTabY + 30f + 28f + (t.Y - (iconInfoTabY + 28f))),
                    Color.White, new Color(35, 35, 66), 0f, Vector2.Zero, Vector2.One * 0.75f, iconInfoTabWidth - 12);

            }
        }

        public static string GetColorCodedString(string text)
        {
            return text.Replace("[0:", $"[c/{new Color(255, 222, 128, 255).Hex3()}:")
                .Replace("[1:", $"[c/{new Color(175, 255, 128, 255).Hex3()}:")
                .Replace("[2:", $"[c/{new Color(175, 225, 255, 255).Hex3()}:");
        }

        public static string GetItemTooltipAsOneLine(int item)
        {
            string text = "";
            var tt = Lang.GetTooltip(item);
            for (int i = 0; i < tt.Lines; i++)
            {
                if (text != "")
                    text += ". ";
                text += tt.GetLine(i);
            }
            return text + ".";
        }

        public void DrawDeliveryTab_DrawItems(SpriteBatch sb, int startX, int startY, ref int x, ref int y, int width, int height, int spriteSize, int maxWidth, Color borderColor, Color backColor, List<int> items)
        {
            foreach (var item in items)
            {
                int iconX = (int)(startX + 208f + x * width);
                int iconY = (int)(startY + 42f + 14f + 8f + y * height);

                DrawDeliveryTab_DrawItem(sb, item, iconX, iconY, spriteSize, maxWidth, borderColor, backColor);

                x++;
                if (x > maxWidth)
                {
                    y++;
                    x = 0;
                }
            }
        }

        public void DrawDeliveryTab_DrawItem(SpriteBatch spriteBatch, int item,
            int iconX, int iconY, int spriteSize, int maxWidth, Color borderColor, Color backColor)
        {
            Main.instance.LoadItem(item);
            var itemTexture = TextureAssets.Item[item].Value;
            Helpers.GetItemDrawData(item, out var frame);
            float scale = 1f;
            int lw = itemTexture.Width > itemTexture.Height ? itemTexture.Width : itemTexture.Height;
            if (lw > spriteSize - 4)
            {
                scale = (spriteSize - 4) / (float)lw;
            }

            var iconRect = new Rectangle(iconX, iconY, spriteSize + 4, spriteSize + 4);
            if (iconRect.Contains(Main.mouseX, Main.mouseY))
            {
                if (hoverItem == null || hoverItem.type != item)
                {
                    hoverItem = new Item();
                    hoverItem.SetDefaults(item);
                }
                RORUI.HoverItem(hoverItem);
                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    selectedItem = item;
                }
                borderColor *= 2;
            }
            Helpers.DrawRectangle(iconRect, borderColor);
            Helpers.DrawRectangle(new Rectangle(iconRect.X + 2, iconRect.Y + 2, iconRect.Width - 4, iconRect.Height - 4), backColor);

            spriteBatch.Draw(itemTexture, new Vector2(iconX + 2 + spriteSize / 2f, iconY + 2 + spriteSize / 2f), null, Color.White,
                0f, frame.Size() / 2f, scale, SpriteEffects.None, 0f);
        }
    }
}