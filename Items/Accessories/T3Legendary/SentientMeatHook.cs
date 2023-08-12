using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Projectiles.Accessory.Visual;
using Terraria.DataStructures;
using RiskOfTerrain.NPCs;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class SentientMeatHook : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add((Type, () => Main.hardMode));
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit)
        {
            if (victim.entity is NPC npc && Main.rand.NextBool(5))
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (RORNPC.Distance(Main.npc[i], npc) <= 400 && !npc.friendly && npc.lifeMax > 5 && npc.damage > 0 && i != npc.whoAmI)
                    {
                        Projectile.NewProjectile(entity.GetSource_Accessory(Item), victim.Center, Vector2.Zero, ModContent.ProjectileType<MeatHookProjDamaging>(), 0, 0, entity.GetProjectileOwnerID(), i, npc.whoAmI);
                    }
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
            {
                player.ROR().showMeatHook = true;

                bool spawnHook = true;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type == ModContent.ProjectileType<MeatHookProjVanity>() && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].active)
                    {
                        spawnHook = false; break;
                    }
                }

                if (spawnHook) { Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<MeatHookProjVanity>(), 0, 0, Owner: player.whoAmI); }
            }
        }

        public override void UpdateVanity(Player player)
        {
            player.ROR().showMeatHook = true;

            bool spawnHook = true;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].type == ModContent.ProjectileType<MeatHookProjVanity>() && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].active)
                {
                    spawnHook = false; break;
                }
            }

            if (spawnHook) { Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<MeatHookProjVanity>(), 0, 0, Owner: player.whoAmI); }
        }
    }
}