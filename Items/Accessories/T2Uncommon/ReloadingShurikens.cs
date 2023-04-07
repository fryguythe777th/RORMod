using Microsoft.Xna.Framework;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Projectiles.Misc;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class ReloadingShurikens : ModAccessory
    {
        public int shurikenCharges;
        public int shurikenRechargeTime;

        public int MaxCharges = 3;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.GreenTier.Add((Type, () => NPC.downedBoss3));
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void OnUnequip(EntityInfo entity)
        {
            entity.ClearBuff(ModContent.BuffType<ShurikenBuff>());
            shurikenCharges = 0;
            shurikenRechargeTime = 0;
        }

        public override void OnEquip(EntityInfo entity)
        {
            entity.AddInfoBuff<ShurikenBuff>();
            shurikenCharges = 0;
            shurikenRechargeTime = 0;
            if (entity.IsMe())
            {
                ShurikenBuff.Brightness = 0f;
                ShurikenBuff.Charges = 0;
                ShurikenBuff.AtMaxCharge = true;
                ShurikenBuff.RechargePercentage = 0f;
            }
        }

        public override void PostUpdateEquips(EntityInfo entity)
        {
            if (shurikenCharges != 0)
            {
                entity.AddInfoBuff<ShurikenBuff>();
            }
            if (shurikenCharges < MaxCharges)
            {
                shurikenRechargeTime++;
                if (shurikenRechargeTime > 120)
                {
                    if (entity.IsMe())
                    {
                        ShurikenBuff.Brightness = 0.75f;
                    }
                    shurikenRechargeTime = 0;
                    shurikenCharges++;
                }
            }
            if (entity.IsMe())
            {
                ShurikenBuff.Brightness *= 0.95f;
                ShurikenBuff.RechargePercentage = shurikenRechargeTime / 120f;
                ShurikenBuff.AtMaxCharge = shurikenCharges >= MaxCharges;
                ShurikenBuff.Charges = shurikenCharges;
            }
        }

        public override void OnUseItem(EntityInfo entity, Item item)
        {
            int max = MaxCharges;
            shurikenCharges = Math.Min(shurikenCharges, max);
            if (entity.IsMe() && !item.IsAir && item.damage > 0 && shurikenCharges > 0 && (shurikenRechargeTime > 20 || shurikenCharges == max))
            {
                var targetPoint = entity.GetTargetPoint();
                if (targetPoint == Vector2.Zero)
                {
                    return;
                }

                shurikenCharges--;
                shurikenRechargeTime = 0;
                if (shurikenCharges <= 0)
                {
                    entity.ClearBuff(ModContent.BuffType<ShurikenBuff>());
                }
                var p = Projectile.NewProjectileDirect(entity.GetSource_Accessory(Item), entity.Center, Vector2.Normalize(targetPoint - entity.Center) * 20f,
                    ModContent.ProjectileType<ReloadingShurikenProj>(), Math.Clamp(entity.GetWeaponDamage(item) * 2, 10, 200), 1f, entity.GetProjectileOwnerID());
                p.DamageType = item.DamageType;
            }
        }
    }
}