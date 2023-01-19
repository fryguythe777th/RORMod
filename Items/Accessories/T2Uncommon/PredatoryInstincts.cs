using RiskOfTerrain.Buffs;
using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class PredatoryInstincts : ModAccessory
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

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance(DamageClass.Generic) += 5;
        }

        public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref int damage, ref float knockBack, ref bool crit)
        {
            if (crit && entity.entity is Player player)
            {
                player.AddBuff(ModContent.BuffType<PredatoryInstinctsBuff>(), 300);
            }
        }
    }
}