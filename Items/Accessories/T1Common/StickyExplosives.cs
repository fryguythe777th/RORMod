using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Projectiles.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    public class StickyExplosives : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.WhiteTier.Add((Type, () => NPC.downedBoss1));
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit)
        {
            if (victim.entity is NPC npc && !npc.immortal)
            {
                entity.GetProc(out float proc);
                if (Main.rand.NextFloat(1f) <= proc && entity.RollLuck(10) == 0)
                {
                    Projectile.NewProjectile(entity.entity.GetSource_Accessory(Item), entity.entity.Center + Main.rand.NextVector2Unit() * 100f, Vector2.Zero, ModContent.ProjectileType<StickyExplosivesProj>(),
                        (int)(hit.Damage * 0.75f * proc), 0f, entity.GetProjectileOwnerID(), npc.whoAmI);
                }
            }
        }
    }
}