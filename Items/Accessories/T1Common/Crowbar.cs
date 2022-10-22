using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    public class Crowbar : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.WhiteTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ROR().accCrowbar = true;
        }

        public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref int damage, ref float knockBack, ref bool crit)
        {
            if ((entity.entity is NPC npc && (npc.life / (float)npc.lifeMax) >= 0.9f) || 
                (entity.entity is Player player && (player.statLife / (float)player.statLifeMax2) >= 0.9f))
            {
                damage = (int)(damage * (1f + 0.25f * Item.stack));
            }
        }
    }
}