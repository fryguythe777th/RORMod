using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace RiskOfTerrain.Buffs
{
    public class AfterburnerBuff : ModBuff
    {
        public static float Brightness;
        public static float RechargePercentage;
        public static int Charges;
        public static bool AtMaxCharge;

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            drawParams.DrawColor *= 1.25f;
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
        {
            var texture = TextureAssets.Buff[Type].Value;
            if (!AtMaxCharge)
            {
                float progress = RechargePercentage;
                int y = (int)(texture.Height * progress);
                var frame = new Rectangle(0, texture.Height - y, texture.Width, y);
                var drawLoc = drawParams.Position + new Vector2(0f, texture.Height - y);
                var drawColor = (drawParams.DrawColor * 1.4f).UseA(drawParams.DrawColor.A);
                spriteBatch.Draw(texture, drawLoc, frame, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(texture, drawLoc, frame, drawColor.UseA(0) * Main.buffAlpha[buffIndex], 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(texture, drawParams.Position, null, drawParams.DrawColor.UseA(0) * ((float)Math.Sin(Brightness * Math.PI) + Brightness), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            var font = FontAssets.ItemStack.Value;
            string text = $"x{Charges}";
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, text, drawParams.TextPosition + new Vector2(16f * 0.9f, 0f), drawParams.DrawColor, Color.Black.UseA(drawParams.DrawColor.A), 0f, new Vector2(font.MeasureString(text).X / 2f, 0f), Vector2.One * 0.8f);
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }
    }
}