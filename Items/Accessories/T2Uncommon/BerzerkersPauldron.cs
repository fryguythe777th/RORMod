using RiskOfTerrain.Buffs;
using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class BerzerkersPauldron : ModAccessory
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

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            if (entity.entity is Player player && !player.HasBuff(ModContent.BuffType<BerzerkerBuff>()))
            {
                player.ROR().berzerkerTimer = 420;
                player.ROR().berzerkerCounter++;
            }
        }
    }
}