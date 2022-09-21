using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Items.Accessories
{
    [AutoloadEquip(EquipType.Front)]
    public class BustlingFungus : ModItem, ItemHooks.IUpdateItemDye
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
            player.ROR().accBustlingFungus = Item;
            player.ROR().bungusDiameter += 280f;
            player.ROR().bungusHealingPercent += 0.075f;
        }

        void ItemHooks.IUpdateItemDye.UpdateItemDye(Player player, bool isNotInVanitySlot, bool isSetToHidden, Item armorItem, Item dyeItem)
        {
            player.ROR().cBungus = dyeItem.dye;
        }
    }
}