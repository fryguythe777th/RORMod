using Microsoft.Xna.Framework;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.NPCs;
using RiskOfTerrain.Projectiles.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T4Boss
{
    public class Shatterspleen : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.BossTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance<GenericDamageClass>() += 5;
        }

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit)
        {
            if (!hit.Crit)
                return;

            if (victim.entity is NPC npc)
            {
                npc.GetGlobalNPC<RORNPC>().bleedShatterspleen = true;
                BleedingDebuff.AddStack(npc, 300, Stacks);
            }
            else if (victim.entity is Player player)
            {
                player.AddBuff(ModContent.BuffType<BleedingDebuff>(), 300);
            }
        }

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            if (info.miscInfo[0] && entity.IsMe())
            {
                Projectile.NewProjectile(entity.GetSource_Accessory(Item), info.Center, Vector2.Normalize(info.Center - entity.Center) * 0.1f,
                    ModContent.ProjectileType<ShatterspleenExplosion>(), info.lifeMax / 4, 6f, entity.GetProjectileOwnerID());
            }
        }
    }
}