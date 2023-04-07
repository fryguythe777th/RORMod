using RiskOfTerrain.Tiles.Furniture;
using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Placeable
{
    public class RustyLockbox : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<RustyLockboxTile>());
            Item.value = Item.sellPrice(silver: 10);
        }
    }
}