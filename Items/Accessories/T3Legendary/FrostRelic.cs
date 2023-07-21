using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Projectiles.Accessory.Damaging;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class FrostRelic : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            for (int i = 0; i < 3; i++)
            {
                Projectile.NewProjectile(entity.GetSource_Accessory(Item), entity.Center, Vector2.Zero, ModContent.ProjectileType<FrostRelicIcicle>(), 3, 0, entity.GetProjectileOwnerID(), i);
            }
        }
    }
}