﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Buffs.Debuff;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class ICBM : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add((Type, () => NPC.downedPlantBoss));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ROR().accICBM = true;
        }
    }
}