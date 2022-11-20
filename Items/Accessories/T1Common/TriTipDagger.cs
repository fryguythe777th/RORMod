using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    [AutoloadEquip(EquipType.Face)]
    public class TriTipDagger : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.WhiteTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, int damage, float knockBack, bool crit)
        {
            entity.GetProc(out float proc);
            if (Main.rand.NextFloat(1f) <= proc && entity.RollLuck(10) == 0)
            {
                BleedingDebuff.AddStack(victim.entity, (int)(180 * proc), 1);
            }
        }
    }
}