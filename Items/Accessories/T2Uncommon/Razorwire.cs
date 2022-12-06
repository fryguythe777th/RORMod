using Microsoft.Xna.Framework;
using RiskOfTerrain.Projectiles.Misc;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class Razorwire : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.GreenTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void Hurt(Player player, RORPlayer ror, bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            if (Main.myPlayer != player.whoAmI)
                return;

            int targetsFound = 0;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (!Main.npc[i].CanBeChasedBy(player))
                {
                    continue;
                }

                if (Vector2.Distance(player.Center, Main.npc[i].Center) < 300f)
                {
                    targetsFound++;
                    Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, (Main.npc[i].Center - player.Center) / 8, ModContent.ProjectileType<RazorwireProj>(),
                        Math.Max(player.GetWeaponDamage(Item), 1), player.HeldItem.knockBack, player.whoAmI, i);
                    if (targetsFound >= 5)
                    {
                        break;
                    }
                }

            }
        }
    }
}