using RORMod.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Items.Placeable
{
    public class Terminal : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<TerminalTile>());
            Item.value = Item.buyPrice(gold: 10);
            Item.maxStack = 99;
            Item.rare = ItemRarityID.LightRed;
        }
    }
}