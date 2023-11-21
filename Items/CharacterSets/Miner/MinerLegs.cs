using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RiskOfTerrain.Graphics;
using RiskOfTerrain.Items.Accessories.T3Legendary;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Miner
{
    [AutoloadEquip(EquipType.Legs)]
    public class MinerLegs : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            ArmorIDs.Legs.Sets.HidesTopSkin[Item.legSlot] = true;
            ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 14;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetAttackSpeed<GenericDamageClass>() += 0.05f;
            player.accRunSpeed += 4;
        }
    }
}