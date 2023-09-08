using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Items.CharacterSets.Miner;
using RiskOfTerrain.UI;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace RiskOfTerrain.Items.CharacterSets.Miner
{
    public class MinerDashBar : UIState
    {
        private UIImage barOutline;
        private Color gradientA;
        private Color gradientB;

        public override void OnInitialize()
        {
            barOutline = new UIImage(ModContent.Request<Texture2D>("RiskOfTerrain/Items/CharacterSets/Miner/DashBarOutline"));
            barOutline.Left.Set(-42, 1f);
            barOutline.Top.Set(35, 0f);
            barOutline.Width.Set(42, 0f);
            barOutline.Height.Set(18, 0f);
            barOutline.HAlign = -0.5f;
            barOutline.VAlign = 0.5f;

            gradientA = new Color(253, 240, 104);
            gradientB = new Color(176, 81, 0);

            Append(barOutline);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.LocalPlayer.HeldItem.ModItem is not MinerPickaxeWeapon || Main.LocalPlayer.ROR().minerDashCharge == 0)
            {
                return;
            }

            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var modPlayer = Main.LocalPlayer.ROR();
            float quotient = (float)modPlayer.minerDashCharge / modPlayer.minerMaxCharge;
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

    public class MinerFuelMeter : UIState
    {
        private UIImage meter;
        private UIImage tick;
        private UIImage cap;

        public override void OnInitialize()
        {
            meter = new UIImage(ModContent.Request<Texture2D>("RiskOfTerrain/Items/CharacterSets/Miner/FuelMeter"));
            meter.Left.Set(-30, 1f);
            meter.Top.Set(-35, 0f);
            meter.Width.Set(30, 0f);
            meter.Height.Set(30, 0f);
            meter.HAlign = -0.5f;
            meter.VAlign = 0.5f;

            Append(meter);

            tick = new UIImage(ModContent.Request<Texture2D>("RiskOfTerrain/Items/CharacterSets/Miner/FuelMeterTick"));
            tick.Left.Set(-16, 1f);
            tick.Top.Set(-19, 0f);
            tick.Width.Set(30, 0f);
            tick.Height.Set(30, 0f);
            tick.HAlign = -0.5f;
            tick.VAlign = 0.5f;

            Append(tick);

            cap = new UIImage(ModContent.Request<Texture2D>("RiskOfTerrain/Items/CharacterSets/Miner/FuelMeterTickCap"));
            cap.Left.Set(-30, 1f);
            cap.Top.Set(-35, 0f);
            cap.Width.Set(30, 0f);
            cap.Height.Set(30, 0f);
            cap.HAlign = -0.5f;
            cap.VAlign = 0.5f;

            Append(cap);
        }

        public override void OnActivate()
        {
            tick.Rotation = MathHelper.Pi;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Main.LocalPlayer.ROR().minerSetBonusActive)
            {
                return;
            }

            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var modPlayer = Main.LocalPlayer.ROR();

            float max = MathHelper.Pi;
            float step = max / 2000f;

            float intendedRot = (step * modPlayer.minerFuel) + MathHelper.Pi;

            if (modPlayer.minerFuel <= 1000)
            {
                tick.Top.Set(-19, 0f);
            }
            else if (modPlayer.minerFuel > 1000 && modPlayer.minerFuel <= 1500)
            {
                tick.Top.Set(-20, 0f);
            }
            else
            {
                tick.Top.Set(-21, 0f);
            }

            tick.Rotation = MathHelper.Lerp(tick.Rotation, intendedRot, 0.06f);
        }
    }

    [Autoload(Side = ModSide.Client)]
    internal class MinerUISystem : ModSystem
    {
        private UserInterface MinerBarInterface;
        internal MinerDashBar minerDashBar;
        private UserInterface MinerFuelInterface;
        internal MinerFuelMeter minerFuelMeter;

        public override void Load()
        {
            minerDashBar = new();
            MinerBarInterface = new();
            MinerBarInterface.SetState(minerDashBar);

            minerFuelMeter = new();
            MinerFuelInterface = new();
            MinerFuelInterface.SetState(minerFuelMeter);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            MinerBarInterface?.Update(gameTime);
            MinerFuelInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "RiskOfTerrain: Miner Dash Bar",
                    delegate
                    {
                        MinerBarInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "RiskOfTerrain: Miner Fuel Meter",
                    delegate
                    {
                        MinerFuelInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}