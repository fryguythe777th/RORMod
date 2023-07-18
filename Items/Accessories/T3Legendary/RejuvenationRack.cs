using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Buffs.Debuff;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class RejuvenationRack : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add((Type, () => Main.hardMode));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public int savedHealth;

        public override void OnEquip(EntityInfo entity)
        {
            if (entity.entity is Player player)
            {
                savedHealth = player.statLife;
            }
        }

        public bool beginningState = true;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!beginningState)
            {
                if (player.statLife > savedHealth)
                {
                    player.statLife += (int)MathHelper.Min((float)(player.statLife - savedHealth), (float)(player.statLifeMax2 - player.statLife));
                }
            }
            else
            {
                beginningState = false;
            }

            savedHealth = player.statLife;
        }
    }
}