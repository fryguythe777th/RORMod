using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Items.Accessories.T1Common
{
    [AutoloadEquip(EquipType.Shoes)]
    public class PaulsGoatHoof : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.WhiteTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.33f;
            player.jumpSpeedBoost += 2f;
            player.ROR().bootSpeed += 2f;
            player.ROR().gLegSounds = !hideVisual;
        }
    }
}