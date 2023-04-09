using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Projectiles.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit)
        {
            if (victim.entity is NPC npc && entity.entity is Player player && (hit.Damage >= npc.lifeMax * 0.6 || hit.InstantKill) && !npc.friendly && !npc.SpawnedFromStatue && npc.lifeMax > 5 && procCooldown == 600)
            {
                int i = Projectile.NewProjectile(entity.entity.GetSource_Accessory(Item), new Vector2(npc.Center.X, npc.Center.Y - 20), Vector2.Zero, ModContent.ProjectileType<KjarosBandTornado>(), 5, 0, Owner: player.whoAmI);
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