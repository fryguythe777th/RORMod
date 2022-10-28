using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.NPCs;
using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    public class ArmorPiercingRounds : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.WhiteTier.Add((Type, () => NPC.downedBoss2));
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref int damage, ref float knockBack, ref bool crit)
        {
            if (victim.entity is NPC target && (target.boss || RORNPC.CountsAsBoss.Contains(target.type)))
                damage = (int)(damage * (1f + Stacks * 0.1f));
        }
    }
}