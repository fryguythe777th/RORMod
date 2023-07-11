using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    [AutoloadEquip(EquipType.Waist)]
    public class Gasoline : ModAccessory
    {
        public static HashSet<int> FireDebuffsForGasolineDamageOverTime { get; private set; }

        public int killDelay;

        public override void Load()
        {
            FireDebuffsForGasolineDamageOverTime = new HashSet<int>()
            {
                BuffID.OnFire,
                BuffID.OnFire3,
            };
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.WhiteTier.Add((Type, () => NPC.downedSlimeKing));
        }

        public override void Unload()
        {
            FireDebuffsForGasolineDamageOverTime?.Clear();
            FireDebuffsForGasolineDamageOverTime = null;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (killDelay > 0)
                killDelay--;
        }

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            if (entity.IsMe() && killDelay <= 0 && !info.friendly && !info.spawnedFromStatue && info.lifeMax > 5 && entity.entity is Player player)
            {
                killDelay = 120;
                Projectile.NewProjectile(player.GetSource_Accessory(Item), info.position + new Vector2(info.width / 2f, info.height / 2f),
                    new Vector2(0f, -1f), ModContent.ProjectileType<GasolineProj>(), Math.Clamp((int)(info.lastHitDamage * 0.5f), 10, 200), 3f, entity.GetProjectileOwnerID());
            }
        }
    }
}