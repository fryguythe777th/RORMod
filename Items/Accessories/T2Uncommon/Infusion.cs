using System;
using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class Infusion : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.GreenTier.Add((Type, () => NPC.downedBoss1));
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2);
        }

        public int ExtraMaxHp = 0;
        public int WearOffTimer = 0;

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            if (ExtraMaxHp < 50)
                ExtraMaxHp++;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += ExtraMaxHp;
            
            if (WearOffTimer == 1000)
            {
                if (ExtraMaxHp > 0)
                {
                    int subAmount = (int)Math.Round(ExtraMaxHp * 0.20) + 1;
                    ExtraMaxHp -= subAmount;
                    CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 3, 3), Color.Red, -subAmount, false);
                }
                WearOffTimer = 0;
            }
            WearOffTimer++;
        }
    }
}