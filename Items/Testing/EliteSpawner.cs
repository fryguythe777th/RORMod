using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Elites;
using Terraria.ID;
using RiskOfTerrain.NPCs;
using System.Collections.Generic;

namespace RiskOfTerrain.Items.Testing
{
    public class EliteSpawner : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.RegenerationPotion);
            Item.consumable = false;
        }

        public int addedPrefixIndex = 0;

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (addedPrefixIndex == 7)
                {
                    addedPrefixIndex = 0;
                }
                else
                {
                    addedPrefixIndex++;
                }

                int i = CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 1, 1), Color.BlanchedAlmond, 0);
                string t = "you ucked up";

                switch (addedPrefixIndex)
                {
                    case 0:
                        t = "Blazing";
                        break;
                    case 1:
                        t = "Celestine";
                        break;
                    case 2:
                        t = "Glacial";
                        break;
                    case 3:
                        t = "Malachite";
                        break;
                    case 4:
                        t = "Mending";
                        break;
                    case 5:
                        t = "Overloading";
                        break;
                    case 6:
                        t = "Perfected";
                        break;
                    case 7:
                        t = "Voidtouched";
                        break;
                    default:
                        break;
                }

                Main.combatText[i].text = t;
                return true;
            }
            else
            {
                Projectile.NewProjectile(player.GetSource_FromThis(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<EliteSpawningProj>(), 0, 0, ai0: addedPrefixIndex, Owner: player.whoAmI);

                return true;
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            //makes it so shimmering ELITINIZER will give you BUNGUS
            CreateRecipe()
                .AddCustomShimmerResult(ModContent.ItemType<Accessories.T1Common.BustlingFungus>())
                .Register();
        }

        //TODOLIST
        // - SCORPION W/ POPPER EFFECT
    }

    public class EliteSpawningProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.timeLeft = 2;
            Projectile.alpha = 255;
            Projectile.damage = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.width = 10;
            Projectile.height = 10;
        }

        public override string Texture => RiskOfTerrain.BlankTexture;

        public override void AI()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].Hitbox.Intersects(Projectile.Hitbox))
                {
                    NPC target = Main.npc[i];
                    var l = new List<EliteNPCBase>(RORNPC.RegisteredElites);
                    target.GetGlobalNPC(l[(int)Projectile.ai[0]]).Active = true;
                    target.GetElitePrefixes(out var myPrefixes);
                    if (myPrefixes.Count > 0)
                    {
                        target.netUpdate = true;
                        target.ROR().syncLifeMax = true;
                    }
                    foreach (var p in myPrefixes)
                    {
                        p.OnBecomeElite(target);
                    }
                }
            }
        }
    }
}