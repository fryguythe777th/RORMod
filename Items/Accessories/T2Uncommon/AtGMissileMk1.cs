using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Projectiles.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class AtGMissileMk1 : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.GreenTier.Add((Type, () => NPC.downedMechBossAny));
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 2, silver: 50);
        }

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, int damage, float knockBack, bool crit)
        {
            if (victim.entity is NPC npc && npc.immortal)
                return;

            entity.GetProc(out float proc);
            if (Main.rand.NextFloat(1f) <= proc && entity.RollLuck(10) == 0)
            {
                Projectile.NewProjectile(entity.entity.GetSource_Accessory(Item), entity.entity.Center, new Vector2(0f, -12f).RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)), ModContent.ProjectileType<AtGMissileProj>(),
                    (int)(damage * proc), 0f, entity.GetProjectileOwnerID(), -10f);
            }
        }
    }
}