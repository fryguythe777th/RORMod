using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Projectiles.Accessory.Utility;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    [AutoloadEquip(EquipType.Back)]
    public class Warbanner : ModAccessory
    {
        public int timer;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.WhiteTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void OnUnequip(EntityInfo entity)
        {
            timer = 0;
        }

        public override void PostUpdate(EntityInfo entity)
        {
            timer++;
            if (timer > Math.Max(7200 / Item.stack, 600))
            {
                if (entity.IsMe() && entity.entity is Player player)
                {
                    Projectile.NewProjectile(player.GetSource_Accessory(Item), entity.entity.Center - new Vector2(0f, 30f), Vector2.UnitY, 
                        ModContent.ProjectileType<WarbannerProj>(), 0, 0f, entity.GetProjectileOwnerID());
                }
                timer = 0;
            }
        }
    }
}