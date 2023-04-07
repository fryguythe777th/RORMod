using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class KjarosBand : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.GreenTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 3);
        }

        public int procCooldown;

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            if (info.lastHitDamage >= info.lifeMax * 0.6 && !info.friendly && !info.spawnedFromStatue && info.lifeMax > 5 && procCooldown == 600)
            {
                int i = Projectile.NewProjectile(entity.entity.GetSource_Accessory(Item), info.Center, Vector2.Zero, ProjectileID.SandnadoHostile, 20, 0);
                Main.projectile[i].hostile = false; 
                Main.projectile[i].friendly = true;
                Main.projectile[i].owner = Main.LocalPlayer.whoAmI;
                procCooldown = 0;
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (procCooldown < 600)
                procCooldown++;
        }
    }
}