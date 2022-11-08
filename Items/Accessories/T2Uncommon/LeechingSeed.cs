using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class LeechingSeed : ModAccessory
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
            if (entity.entity is NPC npc && npc.active && npc.life < npc.lifeMax)
            {
                npc.life++;
            }
            else if (entity.entity is Player player && player.active && player.statLife < player.statLifeMax && victim.entity is NPC ditter && ditter.type != NPCID.TargetDummy && ditter.CountsAsACritter == false && ditter.SpawnedFromStatue == false)
            {
                player.statLife++;
            }
        }
    }
}