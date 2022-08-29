using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Items.Accessories
{
    public class MonsterTooth : ModItem
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
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 50);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            RORPlayer rorp = player.GetModPlayer<RORPlayer>();
            rorp.accMonsterTooth = true;
        }
    }
}