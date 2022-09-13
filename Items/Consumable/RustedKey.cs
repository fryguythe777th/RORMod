using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Items.Consumable
{
    public class RustedKey : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GoldenKey);
            Item.rare = ItemRarityID.Blue;
        }

        public override void UpdateInventory(Player player)
        {
            player.ROR().checkRustedKey = true;
        }
    }
}