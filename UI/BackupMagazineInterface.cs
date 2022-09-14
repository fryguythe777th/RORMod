using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace RORMod.UI
{
    public class BackupMagazineInterface : ModSystem
    {
        public int TimeActive;
        public float Opacity;
        public float Rotation;

        public override void Load()
        {
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (Opacity > 0f)
            {
                TimeActive++;
                if (TimeActive > 20)
                {
                    Opacity -= 0.1f;
                    if (Opacity < 0f)
                        Opacity = 0f;
                }
            }
            else
            {
                TimeActive = 0;
                Rotation = 0f;
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (Opacity > 0f)
            {
                int index = layers.FindIndex((l) => l.Name == "Vanilla: Wire Selection");
                if (index != -1)
                    layers.Insert(index + 1, new LegacyGameInterfaceLayer("RORMod: Backup Magazine", DrawInterface, InterfaceScaleType.UI));
            }
        }

        public bool DrawInterface()
        {
            var player = Main.LocalPlayer;
            if (!player.ROR().ammoSwapVisible)
            {
                return true;
            }

            var ui = ModContent.Request<Texture2D>($"{RORMod.AssetsPath}UI/BackupMagazine").Value;
            var heldItem = player.HeldItem;
            var items = new List<Item>();
            for (int i = Main.InventoryAmmoSlotsStart; i < Main.InventoryAmmoSlotsStart + Main.InventoryAmmoSlotsCount; i++)
            {
                if (player.inventory[i].IsAir || player.inventory[i].ammo != heldItem.useAmmo)
                    continue;
                items.Add(player.inventory[i]);
            }

            if (TimeActive <= 1)
            {
                Rotation = MathHelper.TwoPi / items.Count;
            }
            Rotation *= 0.8f;
            if (Rotation < 0.01f)
            {
                Rotation = 0f;
            }

            var buttonFrame = new Rectangle(0, 0, 40, 40);
            var mousePos = new Vector2(Main.mouseX + 8f, Main.mouseY + 8f);
            var origin = buttonFrame.Size() / 2f;
            var color = new Color(200, 200, 200, 222) * Opacity;
            for (int i = 0; i < items.Count; i++)
            {
                var v = (MathHelper.TwoPi / items.Count * i - Rotation + MathHelper.PiOver2).ToRotationVector2();
                var r = buttonFrame;
                var drawCoords = mousePos + v * buttonFrame.Width;
                if (i == 0)
                {
                    r.Y += buttonFrame.Height + 2;
                    float val = 1f - Rotation;
                    if (val > 0f)
                    {
                        Main.spriteBatch.Draw(ui, drawCoords + new Vector2(-buttonFrame.Height - (1f - val) * 24f, 0f), new Rectangle(0, 84, 18, 28), Color.White * Opacity * Opacity * val, MathHelper.PiOver2, new Vector2(9f, 14f), 1f, SpriteEffects.None, 0f);
                        Main.spriteBatch.Draw(ui, drawCoords + new Vector2(buttonFrame.Height + (1f - val) * 24f + 2f, 0f), new Rectangle(0, 84, 18, 28), Color.White * Opacity * Opacity * val, -MathHelper.PiOver2, new Vector2(9f, 14f), 1f, SpriteEffects.None, 0f);
                    }
                }

                Main.instance.LoadItem(items[i].type);
                var texture = TextureAssets.Item[items[i].type];
                items[i].GetItemDrawData(out var itemFrame);
                Main.spriteBatch.Draw(ui, drawCoords, r, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                float itemScale = 1f;
                int wH = itemFrame.Width > itemFrame.Height ? itemFrame.Width : itemFrame.Height;
                if (wH > 30)
                {
                    itemScale = 30f / wH;
                }
                Main.spriteBatch.Draw(texture.Value, drawCoords + new Vector2(2f), itemFrame, Color.Black * Opacity, 0f, itemFrame.Size() / 2f, itemScale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture.Value, drawCoords, itemFrame, Color.White * Opacity, 0f, itemFrame.Size() / 2f, itemScale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}