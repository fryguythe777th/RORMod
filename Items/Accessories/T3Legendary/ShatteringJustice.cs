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
    public class ShatteringJustice : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add((Type, () => Main.hardMode));
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 54;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref StatModifier damage, ref StatModifier knockBack, ref NPC.HitModifiers modifiers)
        {
            if (victim.entity is NPC target && target.ROR().shatterizationCount < 3 && target.ROR().timeSinceLastHit < 60)
            {
                target.ROR().shatterizationCount++;
                target.AddBuff(ModContent.BuffType<ShatteredDebuff>(), 10800);
            }
        }
    }
}