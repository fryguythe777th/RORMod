using RiskOfTerrain.Tiles.Furniture;
using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Placeable
{
    public class SecurityChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SecurityChestTile>());
            Item.value = Item.sellPrice(silver: 10);
        }
    }
}