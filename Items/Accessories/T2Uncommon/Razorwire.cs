using Microsoft.Xna.Framework;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
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
            Item.ResearchUnlockCount = 1;
            RORItem.GreenTier.Add((Type, () => NPC.downedBoss1));
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void Hurt(Player player, RORPlayer ror, Player.HurtInfo info)
        {
            if (Main.myPlayer != player.whoAmI)
                return;

            int targetsFound = 0;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (!Main.npc[i].CanBeChasedBy(player) || Main.npc[i].IsProbablyACritter() || Main.npc[i].isLikeATownNPC)
                {
                    continue;
                }
                else
                {
                    if (Vector2.Distance(player.Center, Main.npc[i].Center) < 300f || !Main.npc[i].friendly || Main.npc[i].lifeMax != 5 || Main.npc[i].damage != 0)
                    {
                        targetsFound++;
                        Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, (Main.npc[i].Center - player.Center) / 8, ModContent.ProjectileType<RazorwireProj>(),
                            Math.Max((int)(player.HeldItem.damage * 1.6), 1), player.HeldItem.knockBack, player.whoAmI, i);
                        if (targetsFound >= 5)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}