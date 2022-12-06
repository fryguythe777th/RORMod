using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Items;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace RiskOfTerrain.UI.Terminal
{
    public class LogbookPage : TerminalPage, ILoadable
    {
        public static Dictionary<int, Func<string>> CustomTooltipConstructor { get; private set; }
        public static Dictionary<int, Func<string>> DynamicLog { get; private set; }

        public int selectedItem;
        public string log;
        public string tt;
        public Item hoverItem;

        void ILoadable.Load(Mod mod)
        {
            CustomTooltipConstructor = new Dictionary<int, Func<string>>();
            DynamicLog = new Dictionary<int, Func<string>>();
        }

        void ILoadable.Unload()
        {
            CustomTooltipConstructor?.Clear();
            CustomTooltipConstructor = null;
            DynamicLog?.Clear();
            DynamicLog = null;
        }

        public static string GetColoredString(string text)
        {
            return text.Replace("[0:", $"[c/{new Color(255, 222, 128, 255).Hex3()}:")
                .Replace("[1:", $"[c/{new Color(175, 255, 128, 255).Hex3()}:")
                .Replace("[2:", $"[c/{new Color(175, 225, 255, 255).Hex3()}:")
                .Replace("[3:", $"[c/{new Color(255, 128, 128, 255).Hex3()}:");
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

        public void DrawItems(SpriteBatch sb, int startX, int startY, ref int x, ref int y, int width, int height, int spriteSize, int maxWidth, Color borderColor, Color backColor,
            List<ChestDropInfo> items)
        {
            foreach (var item in items)
            {
                int iconX = (int)(startX + 208f + x * width);
                int iconY = (int)(startY + 42f + 14f + 8f + y * height);

                DrawItem(sb, item, iconX, iconY, spriteSize, maxWidth, borderColor, backColor);

                x++;
                if (x > maxWidth)
                {
                    y++;
                    x = 0;
                }
            }
        }

        public void DrawItem(SpriteBatch spriteBatch, ChestDropInfo item,
            int iconX, int iconY, int spriteSize, int maxWidth, Color borderColor, Color backColor)
        {
            Main.instance.LoadItem(item.ItemID);
            var itemTexture = TextureAssets.Item[item.ItemID].Value;
            Helpers.GetItemDrawData(item.ItemID, out var frame);
            float scale = 1f;
            int lw = itemTexture.Width > itemTexture.Height ? itemTexture.Width : itemTexture.Height;
            if (lw > spriteSize - 4)
            {
                scale = (spriteSize - 4) / (float)lw;
            }

            var iconRect = new Rectangle(iconX, iconY, spriteSize + 4, spriteSize + 4);
            if (iconRect.Contains(Main.mouseX, Main.mouseY))
            {
                if (hoverItem == null || hoverItem.type != item.ItemID)
                {
                    hoverItem = new Item();
                    hoverItem.SetDefaults(item.ItemID);
                }
                RORUI.HoverItem(hoverItem);
                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    selectedItem = item.ItemID;
                    tt = null;
                    log = null;
                }
                borderColor *= 2;
            }
            bool obtained = item.CanRoll?.Invoke() != false;
            Helpers.DrawRectangle(iconRect, obtained ? borderColor : (borderColor * 0.75f).UseA(255));
            Helpers.DrawRectangle(new Rectangle(iconRect.X + 2, iconRect.Y + 2, iconRect.Width - 4, iconRect.Height - 4), obtained ? backColor : (backColor * 0.5f).UseA(255));

            spriteBatch.Draw(itemTexture, new Vector2(iconX + 2 + spriteSize / 2f, iconY + 2 + spriteSize / 2f), null, obtained ? Color.White : Color.Black,
                0f, frame.Size() / 2f, scale, SpriteEffects.None, 0f);
        }

        public override void DrawPage(SpriteBatch spriteBatch, Texture2D texture, CalculatedStyle d, Rectangle rect)
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

            DrawItems(spriteBatch, (int)d.X, (int)d.Y, ref x, ref y, width, height, spriteSize, maxWidth, clr * 1.33f, new Color(100, 100, 120, 255), RORItem.WhiteTier);
            DrawItems(spriteBatch, (int)d.X, (int)d.Y, ref x, ref y, width, height, spriteSize, maxWidth, new Color(25, 140, 25, 255) * 2, new Color(66, 90, 60, 255), RORItem.GreenTier);
            DrawItems(spriteBatch, (int)d.X, (int)d.Y, ref x, ref y, width, height, spriteSize, maxWidth, new Color(140, 25, 25, 255) * 2, new Color(90, 60, 60, 255), RORItem.RedTier);

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

                if (string.IsNullOrEmpty(tt))
                {
                    if (CustomTooltipConstructor.TryGetValue(selectedItem, out var dynamicTooltip))
                    {
                        tt = dynamicTooltip();
                    }
                    else if (selectedItem >= Main.maxItemTypes)
                    {
                        var modItem = ItemLoader.GetItem(selectedItem);
                        string detailedTTKey = $"Mods.{modItem.Mod.Name}.ItemTooltip.{modItem.Name}.TerminalTooltip";
                        string detailedTT = Language.GetTextValue(detailedTTKey);
                        if (detailedTT != detailedTTKey)
                        {
                            tt = detailedTT;
                        }
                    }
                    if (string.IsNullOrEmpty(tt))
                    {
                        tt = GetItemTooltipAsOneLine(selectedItem);
                    }

                    tt = GetColoredString(tt);
                }

                t = ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, tt, new Vector2(iconInfoTabX + 12f, iconInfoTabY + 32f),
                    Color.White, new Color(35, 35, 66), 0f, Vector2.Zero, Vector2.One * 0.75f, iconInfoTabWidth - 12);

                int descHeight = (int)(t.Y - iconInfoTabY);
                Helpers.DrawRectangle(new Rectangle(iconInfoTabX + 6, iconInfoTabY + textHeight + descHeight + 22, iconInfoTabWidth - 4, 4), clr);

                if (log == null)
                {
                    log = "";
                    if (DynamicLog.TryGetValue(selectedItem, out var dynamicLog))
                    {
                        log = dynamicLog();
                    }
                    else if (selectedItem >= Main.maxItemTypes)
                    {
                        var modItem = ItemLoader.GetItem(selectedItem);
                        log = Language.GetTextValue($"Mods.{modItem.Mod.Name}.ItemTooltip.{modItem.Name}.TerminalLore");
                    }
                }

                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, log, new Vector2(iconInfoTabX + 12f, iconInfoTabY + 30f + 28f + (t.Y - (iconInfoTabY + 28f))),
                    Color.White, new Color(35, 35, 66), 0f, Vector2.Zero, Vector2.One * 0.75f, iconInfoTabWidth - 12);
            }
        }
    }
}
