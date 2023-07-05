using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Projectiles.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class RunaldsBand : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            RORItem.GreenTier.Add((Type, () => Main.hardMode));
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 3);
        }

        public static int procCooldown;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (procCooldown < 600)
                procCooldown++;

            player.ROR().accRunalds = true;
        }
    }
}