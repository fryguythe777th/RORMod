using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Items.Accessories;

namespace RiskOfTerrain.Items.CharacterSets.Miner
{
    [AutoloadEquip(EquipType.Head)]
    public class MinerHead : ModAccessory
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
            Item.rare = ItemRarityID.Orange;
            Item.defense = 19;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<MinerBody>() && legs.type == ModContent.ItemType<MinerLegs>();
        }

        public override void UpdateEquip(Player player)
        {

        }

        public override void UpdateArmorSet(Player player)
        {
            player.ROR().minerSetBonusActive = true;
            RORPlayer miner = player.ROR();
            if (miner.minerFuel > 0)
            {
                miner.minerFuel--;
            }
        }
    }
}