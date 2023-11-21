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
    public class ResonanceDisc : ModAccessory
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add((Type, () => NPC.downedBoss2));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ROR().accResonanceDisc = true;
        }

        public override void OnEquip(EntityInfo entity)
        {
            if (entity.entity is Player player)
            {
                player.ROR().resDiscID = Projectile.NewProjectile(entity.GetSource_Accessory(Item), entity.Center, Vector2.Zero, ModContent.ProjectileType<ResonanceDiscProj>(), 0, 0, entity.GetProjectileOwnerID());
            }
        }

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            if (entity.entity is Player player)
            {
                Main.projectile[player.ROR().resDiscID].ai[0]++;
            }
        }

        public override void OnUnequip(EntityInfo entity)
        {
            if (entity.entity is Player player)
            {
                Main.projectile[player.ROR().resDiscID].Kill();
            }
        }
    }
}