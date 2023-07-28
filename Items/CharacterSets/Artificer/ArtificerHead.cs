using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Artificer
{
    [AutoloadEquip(EquipType.Head)]
    public class ArtificerHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = false;
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = false;
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
            ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = false;
            ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 6;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ArtificerBody>() && legs.type == ModContent.ItemType<ArtificerLegs>();
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 30;
            player.GetDamage(DamageClass.Magic) += 0.7f;
            player.GetCritChance(DamageClass.Magic) += 0.13f;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.RiskOfTerrain.SetBonuses.ArtificerSet");
            player.manaCost *= 0.9f;
        }
    }
}