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
            Item.ResearchUnlockCount = 1;
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

        public override void OnHitBy(EntityInfo entity, EntityInfo attacker, Player.HurtInfo info)
        {
            if (entity.entity is NPC npc)
            {
                npc.AddBuff(ModContent.BuffType<MedkitBuff>(), (int)Math.Clamp(360 * (npc.life / (float)npc.lifeMax), 120f, 360f));
            }
        }

        public override void Hurt(Player player, RORPlayer ror, Player.HurtInfo info)
        {
            player.AddBuff(ModContent.BuffType<MedkitBuff>(), (int)Math.Clamp(360 * (Math.Clamp(player.statLife - info.Damage, 0, player.statLifeMax2) / (float)player.statLifeMax2), 120f, 360f));
        }
    }
}