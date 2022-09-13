using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Items.Accessories
{
    [AutoloadEquip(EquipType.Front)]
    public class CautiousSlug : ModItem
    {
        public override void Load()
        {
            EquipLoader.AddEquipTexture(Mod, Texture + "_Front_Hide", EquipType.Front, name: "CautiousSlug_Hide");
        }

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
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 50);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ROR().accGlubby = true;
            if (Main.netMode != NetmodeID.Server)
            {
                Item.frontSlot = (player.ROR().glubbyActive > 120) ? (sbyte)EquipLoader.GetEquipSlot(Mod, "CautiousSlug", EquipType.Front) : (sbyte)EquipLoader.GetEquipSlot(Mod, "CautiousSlug_Hide", EquipType.Front);
            }
            player.ROR().glubbyHide = hideVisual;
        }
    }
}