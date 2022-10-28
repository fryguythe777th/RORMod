using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    public class Crowbar : ModAccessory
    {
        public int soundDelay;

        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.WhiteTier.Add((Type, () => NPC.downedBoss2));
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void PostUpdateEquips(EntityInfo entity)
        {
            if (soundDelay > 0)
                soundDelay--;
        }

        public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref int damage, ref float knockBack, ref bool crit)
        {
            if ((victim.entity is NPC npc && (npc.life / (float)npc.lifeMax) >= 0.9f) ||
                (victim.entity is Player player && (player.statLife / (float)player.statLifeMax2) >= 0.9f))
            {
                damage = (int)(damage * (1f + 0.25f * Stacks));
                if (soundDelay <= 0)
                    SoundEngine.PlaySound(RiskOfTerrain.GetSounds("crowbar/proc", 2, 0.1f, 0f, 0.1f), victim.entity.Center);
            }
        }
    }
}