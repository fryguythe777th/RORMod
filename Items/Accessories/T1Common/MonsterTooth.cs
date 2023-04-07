using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Projectiles.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    public class MonsterTooth : ModAccessory
    {
        public int killDelay;

        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.WhiteTier.Add((Type, () => NPC.downedSlimeKing));
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 50);
        }

        public override void UpdateEquip(Player player)
        {
            if (killDelay > 0)
                killDelay--;
        }

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            if (entity.IsMe())
            {
                Projectile.NewProjectile(entity.entity.GetSource_Accessory(Item), info.Center, new Vector2(0f, -2f), ModContent.ProjectileType<MonsterToothProj>(), 0, 0, entity.GetProjectileOwnerID());
            }
        }
    }
}