using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Items.Accessories.T2Uncommon
{
    public class HarvestersScythe : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.GreenTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ROR().accHarvestersScythe += 12;
        }
    }
}