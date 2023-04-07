using RiskOfTerrain.Tiles.Furniture;
using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Placeable
{
    public class LargeChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<LargeChestTile>());
            Item.value = Item.sellPrice(silver: 10);
        }
    }
}