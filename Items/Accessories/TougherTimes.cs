using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Items.Accessories
{
    public class TougherTimes : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
        }

        public override string Texture => "Terraria/ModLoader/Default/UnloadedItem";

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.White;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ROR().accTougherTimes = true;
        }
    }
}