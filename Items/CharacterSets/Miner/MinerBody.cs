using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Miner
{
    [AutoloadEquip(EquipType.Body)]
    public class MinerBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 13;
        }

        public override void UpdateEquip(Player player)
        {
            
        }
    }
}