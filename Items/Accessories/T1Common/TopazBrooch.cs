using RiskOfTerrain.Content.Accessories;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    [AutoloadEquip(EquipType.Neck)]
    public class TopazBrooch : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.WhiteTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ROR().accTopazBrooch = true;
        }

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            if (entity.entity is Player player)
            {
                var ror = player.ROR();
                //ror.barrierLife = Math.Min(ror.barrierLife + 15, player.statLifeMax2);
                ror.barrierLife += 15;
                if (ror.barrierLife > player.statLifeMax2)
                {
                    ror.barrierLife = player.statLifeMax2;
                }
                player.statLife += 15;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Turtle)
                .AddIngredient(ItemID.Topaz, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}