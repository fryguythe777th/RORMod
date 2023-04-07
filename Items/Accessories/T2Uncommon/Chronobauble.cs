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
            Item.ResearchUnlockCount = 1;
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

        public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref StatModifier damage, ref StatModifier knockBack, ref NPC.HitModifiers modifiers)
        {
            victim.AddBuff(ModContent.BuffType<ChronobaubleDebuff>(), 360);
        }
    }
}