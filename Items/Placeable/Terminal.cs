//using RiskOfTerrain.Tiles;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace RiskOfTerrain.Items.Placeable
//{
//    public class Terminal : ModItem
//    {
//        public override void SetStaticDefaults()
//        {
//            Item.ResearchUnlockCount = 1;
//        }

//        public override void SetDefaults()
//        {
//            Item.DefaultToPlaceableTile(ModContent.TileType<TerminalTile>());
//            Item.value = Item.buyPrice(gold: 10);
//            Item.rare = ItemRarityID.LightRed;
//        }

//        public override bool IsLoadingEnabled(Mod mod)
//        {
//            return false;
//        }
//    }
//}