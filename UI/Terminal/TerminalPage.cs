using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace RiskOfTerrain.UI.Terminal
{
    public abstract class TerminalPage : UIElement
    {
        //public TerminalUIState terminal;

        public abstract void DrawPage(SpriteBatch spriteBatch, Texture2D texture, CalculatedStyle d, Rectangle rect);
    }
}
