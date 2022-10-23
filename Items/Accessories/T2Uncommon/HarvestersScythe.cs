using RiskOfTerrain.Content.Accessories;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class HarvestersScythe : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.GreenTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, int damage, float knockBack, bool crit)
        {
            if (crit)
            {
                int heal = 8 + 4 * (Stacks - 1);
                if (entity.entity is Player player)
                {
                    player.Heal(Math.Min(heal, Math.Max((int)player.lifeSteal, 1)));
                    if (player.lifeSteal > 0f)
                    {
                        player.lifeSteal = Math.Max(player.lifeSteal - Math.Clamp(player.statLifeMax2 - player.statLife, 0, heal), 0f);
                    }
                    Main.NewText(player.lifeSteal);
                }
                else if (entity.entity is NPC npc)
                {
                    npc.life += Math.Clamp(npc.lifeMax - npc.life, 0, heal);
                    npc.HealEffect(heal);
                }

                SoundEngine.PlaySound(RiskOfTerrain.GetSounds("healscythe/heal", 5, 0.1f, 0f, 0.1f), entity.entity.Center);
            }
        }
    }
}