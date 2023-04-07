using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class DiosBestFriend : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add((Type, () => Main.hardMode));
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Main.LocalPlayer.ROR().diosCooldown > 0)
            {
                byte a = lightColor.A;
                return (lightColor * 0.5f).UseA(a);
            }
            return null;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.LocalPlayer.difficulty == PlayerDifficultyID.Hardcore)
            {
                tooltips.Insert(RORItem.GetIndex(tooltips, "Consumable"), new TooltipLine(Mod, "Consumable", Language.GetTextValue("LegacyTooltip.35")));
            }
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                tooltips.Add(new TooltipLine(Mod, "Consumable", $"[c/{Colors.AlphaDarken(new Color(235, 20, 20, 255)).Hex3()}:Warning: Buggy in Multiplayer!]"));
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var ror = player.ROR();
            if (ror.diosCooldown > 0 && player.difficulty == PlayerDifficultyID.Hardcore)
            {
                Item.TurnToAir();
                ror.diosCooldown = 0;
            }
            ror.accDiosBestFriend = 36000;
        }
    }
}