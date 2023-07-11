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
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
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

        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
    }
}