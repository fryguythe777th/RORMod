using System;
using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using RiskOfTerrain.Buffs.Debuff;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class Chronobauble : ModAccessory
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
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref int damage, ref float knockBack, ref bool crit)
        {
            victim.AddBuff(ModContent.BuffType<ChronobaubleDebuff>(), 360);
        }
    }
}