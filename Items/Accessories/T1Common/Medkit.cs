using RiskOfTerrain.Buffs;
using RiskOfTerrain.Content.Accessories;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    [AutoloadEquip(EquipType.Waist)]
    public class Medkit : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.WhiteTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 50);
        }

        public override void OnHitBy(EntityInfo entity, EntityInfo attacker, int damage, float knockBack, bool crit)
        {
            if (entity.entity is NPC npc)
            {
                npc.AddBuff(ModContent.BuffType<MedkitBuff>(), (int)Math.Clamp(360 * (npc.life / (float)npc.lifeMax), 120f, 360f));
            }
        }

        public override void Hurt(Player player, RORPlayer ror, bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            player.AddBuff(ModContent.BuffType<MedkitBuff>(), (int)Math.Clamp(360 * (Math.Clamp(player.statLife - damage, 0, player.statLifeMax2) / (float)player.statLifeMax2), 120f, 360f));
        }
    }
}