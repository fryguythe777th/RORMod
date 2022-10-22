using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace RiskOfTerrain.UI
{
    public class PurchasableChestInterface : ModSystem
    {
        public class ChestPurchasePopup
        {
            public int chestX;
            public int chestY;
            public Vector2 chestCenterOffset;
            public float radius;
            public Vector2 position;
            public string text;
            public int textWidth;
            public int textHeight;
            public int popInTimer;
            public Vector2 textScale;
            public Vector2 textOrigin;

            public Rectangle TextHitbox { get => new Rectangle((int)position.X - textWidth / 2, (int)position.Y - textHeight / 2, textWidth, textHeight); }

            public ChestPurchasePopup(int left, int top, int price, int centerX, int centerY, float radius = 240f)
            {
                chestX = left;
                chestY = top;
                chestCenterOffset = new Vector2(centerX * 16f, centerY * 16f);
                if (price >= Item.platinum)
                {
                    text += $"{price / Item.platinum} platinum";
                    price %= Item.platinum;
                }
                if (price >= Item.gold)
                {
                    if (!string.IsNullOrEmpty(text))
                        text += '\n';
                    text += $"{price / Item.gold} gold";
                    price %= Item.gold;
                }
                if (price >= Item.silver)
                {
                    if (!string.IsNullOrEmpty(text))
                        text += '\n';
                    text += $"{price / Item.silver} silver";
                    price %= Item.silver;
                }
                if (price >= Item.copper)
                {
                    if (!string.IsNullOrEmpty(text))
                        text += '\n';
                    text += $"{price} copper";
                }
                textScale = new Vector2(0.5f, 0.2f);
                var font = FontAssets.ItemStack.Value;
                var measurement = font.MeasureString(text);
                textWidth = (int)measurement.X;
                textHeight = (int)measurement.Y;
                textOrigin = measurement / 2f;
                position = new Vector2(left * 16f + chestCenterOffset.X, top * 16f - textOrigin.Y - 4f);
                this.radius = radius / 2f;
            }

            public bool Update()
            {
                if (!Chest.IsLocked(chestX, chestY) || Vector2.Distance(Main.LocalPlayer.Center, new Vector2(chestX * 16f + chestCenterOffset.X, chestY * 16f + chestCenterOffset.Y)) > radius)
                {
                    textScale = Vector2.Lerp(textScale, Vector2.Zero, 0.25f);
                    if (popInTimer < 0)
                        return false;
                    popInTimer--;
                }
                else if (popInTimer < 6)
                {
                    textScale = Vector2.Lerp(textScale, Vector2.One, 0.25f);
                    popInTimer++;
                }
                return true;
            }
        }

        public static Dictionary<Point, ChestPurchasePopup> PurchasePopups { get; private set; }

        public override void Load()
        {
            PurchasePopups = new Dictionary<Point, ChestPurchasePopup>();
        }

        public override void OnWorldLoad()
        {
            PurchasePopups?.Clear();
        }

        public override void OnWorldUnload()
        {
            PurchasePopups?.Clear();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (PurchasePopups.Count > 0)
            {
                var removePoints = new List<Point>();
                foreach (var popUp in PurchasePopups)
                {
                    if (!popUp.Value.Update())
                    {
                        removePoints.Add(popUp.Key);
                    }
                }
                foreach (var p in removePoints)
                {
                    PurchasePopups.Remove(p);
                }
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (PurchasePopups.Count > 0)
            {
                int index = layers.FindIndex((l) => l.Name == "Vanilla: Entity Health Bars");
                if (index != -1)
                    layers.Insert(index + 1, new LegacyGameInterfaceLayer("RiskOfTerrain: Purchasable Chest", DrawInterface, InterfaceScaleType.Game));
            }
        }

        public bool DrawInterface()
        {
            var font = FontAssets.ItemStack.Value;
            foreach (var popUp in PurchasePopups.Values)
            {
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, popUp.text,
                    popUp.position - Main.screenPosition, Color.Yellow, (Color.Yellow * 0.2f).UseA(100), 0f, popUp.textOrigin, popUp.textScale);
            }
            return true;
        }
    }
}
