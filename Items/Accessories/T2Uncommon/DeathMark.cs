using Microsoft.Xna.Framework;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Projectiles.Accessory.Visual;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class DeathMark : ModAccessory
    {
        public static HashSet<int> DoesntCountAsDebuff { get; private set; }

        public override void Load()
        {
            DoesntCountAsDebuff = new HashSet<int>();
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.GreenTier.Add((Type, () => Main.hardMode));
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit)
        {
            if (victim.GetBuffs(out var buffTypes, out var buffTimes, out int maxBuffs) && victim.entity is NPC npc)
            {
                int buffCount = 0;
                for (int i = 0; i < npc.buffType.Length; i++)
                {
                    if (npc.buffType[i] != 0 && Main.debuff[npc.buffType[i]] && !DoesntCountAsDebuff.Contains(npc.buffType[i]))
                    {
                        buffCount++;
                    }
                    if (buffCount >= 2)
                    {
                        if (!victim.HasBuff(ModContent.BuffType<DeathMarkDebuff>()) && entity.entity is Player player)
                        {
                            var p = Projectile.NewProjectileDirect(player.GetSource_Accessory(Item), new Vector2(victim.entity.position.X + victim.entity.width / 2f, victim.entity.position.Y - 100f),
                                new Vector2(0f, -2f), ModContent.ProjectileType<DeathMarkProj>(), 0, 0f, entity.GetProjectileOwnerID(), victim.entity.whoAmI);
                            if (victim.entity is NPC)
                            {
                                p.ai[0] = victim.entity.whoAmI;
                                p.netUpdate = true;
                            }
                        }
                        victim.AddBuff(ModContent.BuffType<DeathMarkDebuff>(), 420);
                        break;
                    }
                }
            }
        }
    }
}