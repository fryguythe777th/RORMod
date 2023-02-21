using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Projectiles.Misc;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    public class BundleOfFireworks : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.WhiteTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            if (entity.IsMe() && entity.OwnedProjectilesCountLong(ModContent.ProjectileType<FireworksSpawner>()) <= 0 && !info.friendly && !info.spawnedFromStatue && info.lifeMax > 5)
            {
                Projectile.NewProjectile(entity.entity.GetSource_Accessory(Item), entity.entity.Center, Vector2.Zero, ModContent.ProjectileType<FireworksSpawner>(),
                    Math.Clamp((int)(info.lastHitDamage * (Stacks * 0.5f)), 10, 100) / 8, 0, entity.GetProjectileOwnerID(), 8f + 4f * (Stacks - 1));
            }
        }
    }
}