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
    public class SacrificialDagger : ModAccessory
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

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            if (info.aistyle != 6 && info.lastHitProjectile != ModContent.ProjectileType<SacrificialProj>())
            {
                for (int i = 0; i < 3; i++)
                {
                    int vert = 0;
                    if (i == 1)
                        vert = 10;
                    Vector2 pos = new Vector2((info.Center.X - 15) + (15 * i), info.Center.Y - vert);
                    Projectile.NewProjectile(entity.GetSource_Accessory(Item), pos, Vector2.Zero, ModContent.ProjectileType<SacrificialProj>(), 40, 0, entity.GetProjectileOwnerID());
                }
            }
        }
    }
}