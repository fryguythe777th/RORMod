using System.Collections.Generic;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Content.Accessories;
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

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit)
        {
            if (victim.entity is NPC npc)
            {
                if (npc.life + hit.Damage == npc.lifeMax)
                {
                    entity.AddBuff(ModContent.BuffType<WarHornBuff>(), 60);
                }
            }
        }
    }
}