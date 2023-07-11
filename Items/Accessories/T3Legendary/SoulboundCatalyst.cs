using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Projectiles.Misc;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class SoulboundCatalyst : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add((Type, () => Main.hardMode));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public int soulIndex = 0;
        public int savedLife;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ROR().boundSoulRotTick++;
            player.ROR().releaseTheGhosts = false;

            if (savedLife > player.statLife)
            {
                player.ROR().releaseTheGhosts = true;
                player.ROR().boundSoulCount = 0;
                soulIndex = 0;
            }

            savedLife = player.statLife;
        }

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            if (!info.friendly && info.lifeMax > 5 && !info.spawnedFromStatue && entity.entity is Player player)
            {
                Projectile.NewProjectile(entity.GetSource_Accessory(Item), info.Center, Vector2.Zero, ModContent.ProjectileType<BoundSoul>(), 0, 0, entity.GetProjectileOwnerID(), soulIndex);
                player.ROR().boundSoulCount++;
                soulIndex++;
            }
        }
    }
}