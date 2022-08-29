using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Items.Accessories
{
    public class Shatterspleen : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ROR().accShatterspleen = true;
            player.GetCritChance<GenericDamageClass>() += 5;
        }
    }
}