using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Items.Testing;
using RiskOfTerrain.Projectiles.Accessory.Utility;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    [AutoloadEquip(EquipType.Front)]
    public class BustlingFungus : ModAccessory, ItemHooks.IUpdateItemDye
    {
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

        public override void PostUpdate(EntityInfo entity)
        {
            if (entity.IdleTime() > 60 && entity.entity is Player player)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<BustlingFungusProj>() && entity.OwnsThisProjectile(Main.projectile[i]))
                    {
                        UpdateProjectile(entity, Main.projectile[i]);
                        return;
                    }
                }
                UpdateProjectile(entity, Projectile.NewProjectileDirect(player.GetSource_Accessory(Item), entity.entity.Center, Vector2.Zero,
                    ModContent.ProjectileType<BustlingFungusProj>(), 0, 0f, entity.GetProjectileOwnerID()));
            }
        }

        public void UpdateProjectile(EntityInfo entity, Projectile projectile)
        {
            projectile.scale = MathHelper.Lerp(projectile.scale, 312f, 0.2f);
            projectile.Center = entity.entity.Center;
            var bungus = (BustlingFungusProj)projectile.ModProjectile;
            bungus.accessoryActive = true;
            bungus.regenPercent = 0.2f;
        }

        void ItemHooks.IUpdateItemDye.UpdateItemDye(Player player, bool isNotInVanitySlot, bool isSetToHidden, Item armorItem, Item dyeItem)
        {
            player.ROR().cBungus = dyeItem.dye;
        }
    }
}