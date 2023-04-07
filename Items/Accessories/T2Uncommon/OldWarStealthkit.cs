using System.Collections.Generic;
using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class OldWarStealthkit : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
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

        public int stealthModeCooldown = 0;
        public int stealthSpeedBoostCounter = 0;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.statLife <= player.statLifeMax * 0.25 && stealthModeCooldown <= 0)
            {
                player.AddBuff(BuffID.Invisibility, 300);
                stealthSpeedBoostCounter = 300;
                stealthModeCooldown = 1800;
            }
            else
            {
                stealthModeCooldown--;
            }

            if (stealthSpeedBoostCounter > 0)
            {
                player.moveSpeed *= 1.5f;
                player.maxRunSpeed *= 1.3f;
                stealthSpeedBoostCounter--;
            }
        }
    }
}