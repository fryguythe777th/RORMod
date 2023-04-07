using Microsoft.Xna.Framework;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Projectiles.Misc;
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
            RORItem.GreenTier.Add((Type, () => NPC.downedBoss1));
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
            if (victim.GetBuffs(out var buffTypes, out var buffTimes, out int maxBuffs))
            {
                int buffCount = 0;
                for (int i = 0; i < maxBuffs; i++)
                {
                    if (buffTypes[i] != 0 && Main.debuff[buffTypes[i]] && DoesntCountAsDebuff.Contains(buffTypes[i]))
                    {
                        buffCount++;
                    }
                    if (buffCount >= 2)
                    {
                        if (!entity.HasBuff(ModContent.BuffType<DeathMarkDebuff>()))
                        {
                            var p = Projectile.NewProjectileDirect(entity.entity.GetSource_Accessory(Item), new Vector2(entity.entity.position.X + entity.entity.width / 2f, entity.entity.position.Y - 100f),
                                new Vector2(0f, -2f), ModContent.ProjectileType<DeathMarkProj>(), 0, 0f, entity.GetProjectileOwnerID(), -1f);
                            if (entity.entity is NPC)
                            {
                                p.ai[0] = entity.entity.whoAmI;
                                p.netUpdate = true;
                            }
                        }
                        entity.AddBuff(ModContent.BuffType<DeathMarkDebuff>(), 420);
                        break;
                    }
                }
            }
        }
    }
}