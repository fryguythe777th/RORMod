using Microsoft.Xna.Framework;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Projectiles.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class Bandolier : ModAccessory
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
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1, silver: 50);
        }

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            if (info.lifeMax > 5 && info.friendly == false && info.spawnedFromStatue == false && Main.rand.NextBool(5))
            {
                Projectile.NewProjectile(Item.GetSource_Accessory(Item), info.Center, Vector2.Zero, ModContent.ProjectileType<BandolierProj>(), 0, 0, ai1: info.lifeMax);
            }
        }
        public override bool CanConsumeAmmo(Player player, RORPlayer ror)
        {
            return Main.rand.NextFloat(1f) < (1f - Stacks * 0.1f);
        }
    }
}