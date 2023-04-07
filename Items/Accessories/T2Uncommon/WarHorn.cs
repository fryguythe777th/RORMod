using System.Collections.Generic;
using RiskOfTerrain.Buffs;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class WarHorn : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.GreenTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1, silver: 50);
        }

        public int playerBuffCount;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (PlayerInput.Triggers.Current.QuickBuff)
            {
                if (player.CountBuffs() > playerBuffCount)
                {
                    player.AddBuff(ModContent.BuffType<WarHornBuff>(), 480);
                }
            }

            playerBuffCount = player.CountBuffs();
        }
    }
}