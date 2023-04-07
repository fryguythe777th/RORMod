using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    public class Crowbar : ModAccessory
    {
        public int soundDelay;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
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

        public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref StatModifier damage, ref StatModifier knockBack, ref NPC.HitModifiers modifiers)
        {
            if ((victim.entity is NPC npc && (npc.life >= npc.lifeMax * 0.9f)) ||
                (victim.entity is Player player && (player.statLife >= player.statLifeMax * 0.9f)))
            {
                modifiers.ScalingBonusDamage += 0.25f;
                if (soundDelay <= 0)
                    SoundEngine.PlaySound(RiskOfTerrain.GetSounds("crowbar/proc", 2, 0.1f, 0f, 0.1f), victim.entity.Center);
            }
        }
    }
}