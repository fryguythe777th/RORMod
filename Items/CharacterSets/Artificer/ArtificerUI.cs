using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace RiskOfTerrain.Items.CharacterSets.Artificer
{
    public class ArtificerChargeBar : UIState
    {
        private UIImage barOutline;
        private Color gradientA;
        private Color gradientB;
        private bool centerOnPlayer;

        public override void OnInitialize()
        {
            barOutline = new UIImage(ModContent.Request<Texture2D>("RiskOfTerrain/Items/CharacterSets/Artificer/ChargeBarOutline"));
            barOutline.Left.Set(/*-(38 + (Main.screenWidth / 2))*/0, 1f);
            barOutline.Top.Set(/*35 + Main.screenHeight / 2*/0, 0f);
            barOutline.Width.Set(42, 0f);
            barOutline.Height.Set(18, 0f);

            gradientA = new Color(255, 187, 96);
            gradientB = new Color(82, 217, 255);

            Append(barOutline);
        }

        public override void OnActivate()
        {
            base.OnActivate();
            centerOnPlayer = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!centerOnPlayer)
            {
                return;
            }
            centerOnPlayer = false;
            Left.Set(-Main.screenWidth / 2, 0f);
            Top.Set(Main.screenHeight / 2, 0f);
            Recalculate();
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.LocalPlayer.HeldItem.ModItem is not ArtificerBoltWeapon)
            {
                return;
            }

            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var modPlayer = Main.LocalPlayer.ROR();
            float quotient = (float)modPlayer.artificerCharge / ArtificerBoltWeapon.ChargeMaximum;
            quotient = Utils.Clamp(quotient, 0f, 1f);

            Rectangle hitbox = barOutline.GetInnerDimensions().ToRectangle();
            hitbox.X += 6;
            hitbox.Width -= 12;
            hitbox.Y += 6;
            hitbox.Height -= 12;

            int left = hitbox.Left;
            int right = hitbox.Right;
            int steps = (int)((right - left) * quotient);
            for (int i = 0; i < steps; i += 1)
            {
                float percent = (float)i / (right - left);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
            }
        }
    }

    [Autoload(Side = ModSide.Client)]
    internal class ArtificerUISystem : ModSystem
    {
        private UserInterface ArtificerBarInterface;
        internal ArtificerChargeBar artificerChargeBar;

        public override void Load()
        {
            artificerChargeBar = new();
            ArtificerBarInterface = new();
            ArtificerBarInterface.SetState(artificerChargeBar);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            ArtificerBarInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "RiskOfTerrain: Artificer Charge Bar",
                    delegate
                    {
                        ArtificerBarInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}