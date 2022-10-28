using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Tiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace RiskOfTerrain.UI.Terminal
{
    public class TerminalUIState : RORUIState
    {
        public static Dictionary<int, Func<string>> CustomTooltipConstructor { get; private set; }
        public static Dictionary<int, Func<string>> DynamicLog { get; private set; }

        public LogbookPage page;

        public override void OnInitialize()
        {
            OverrideSamplerState = SamplerState.LinearClamp;

            Width.Set(0, 0.5f);
            Height.Set(0, 0.526f);
            MinWidth.Set(700, 0f);
            MinHeight.Set(400, 0f);
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
            if (Main.playerInventory || Main.LocalPlayer.controlInv && Main.LocalPlayer.releaseInventory || !Main.LocalPlayer.IsTileTypeInInteractionRange(ModContent.TileType<TerminalTile>()))
            {
                RORUI.DynamicInterface.SetState(null);
                SoundEngine.PlaySound(SoundID.MenuClose);
            }
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

            var texture = ModContent.Request<Texture2D>($"{RiskOfTerrain.AssetsPath}UI/Terminal", AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(d.X, d.Y), new Rectangle(27, 1, 1, 11), Color.White, 0f, Vector2.Zero, new Vector2(d.Width, 2f), SpriteEffects.None, 0f);

            spriteBatch.Draw(texture, new Vector2(d.X, d.Y), new Rectangle(2, 1, 23, 11), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, new Vector2(d.X + d.Width - 60, d.Y), new Rectangle(30, 1, 30, 11), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            Helpers.DrawRectangle(new Rectangle((int)d.X + 12, (int)d.Y + 27, (int)d.Width - 24, 24), new Color(30, 30, 60, 255));
            DrawHomePage(spriteBatch, texture, d, rect);
            page?.DrawPage(spriteBatch, texture, d, rect);
            base.DrawSelf(spriteBatch);
        }

        public void DrawHomePage(SpriteBatch spriteBatch, Texture2D texture, CalculatedStyle d, Rectangle rect)
        {
            spriteBatch.Draw(texture, new Vector2(d.X + 12, d.Y + 42 + 14), new Rectangle(367, 399, 77, 19), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, new Vector2(d.X + 12, d.Y + 42 * 2 + 14), new Rectangle(452, 399, 77, 19), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, new Vector2(d.X + 12, d.Y + 42 * 3 + 14), new Rectangle(367, 426, 77, 19), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            if (Main.mouseLeft && Main.mouseLeftRelease && new Rectangle((int)(d.X + 12f), (int)(d.Y + 42f * 3f + 14f), 77 * 2, 19 * 2).Contains(Main.mouseX, Main.mouseY))
            {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                page = new LogbookPage() { terminal = this, };
            }
        }
    }
}