using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Items.Accessories.T2Uncommon
{
    public class Razorwire : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.GreenTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ROR().accRazorwire = Item;
        }
    }
}