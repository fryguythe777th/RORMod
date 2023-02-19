using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Projectiles.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class WillOTheWisp : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.GreenTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            Projectile.NewProjectile(entity.entity.GetSource_FromThis(), info.Center, Vector2.Zero, ModContent.ProjectileType<WilloExplosion>(), 0, 0);
            //make it spawn higher, make it owned by player, make it not have sprite issues, make it not spawn from critters
        }
    }
}