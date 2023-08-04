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
                if (addedPrefixIndex == 8)
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
                        t = "Ghostly";
                        break;
                    case 3:
                        t = "Glacial";
                        break;
                    case 4:
                        t = "Malachite";
                        break;
                    case 5:
                        t = "Mending";
                        break;
                    case 6:
                        t = "Overloading";
                        break;
                    case 7:
                        t = "Perfected";
                        break;
                    case 8:
                        t = "???";
                        break;
                    case 9:
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

        //public override void AddRecipes()
        //{
        //    CreateRecipe()
        //        //makes it so shimmering ELITINIZER will give you BUNGUS
        //        .AddCustomShimmerResult(ModContent.ItemType<Accessories.T1Common.BustlingFungus>())
        //        .AddIngredient(ModContent.ItemType<Accessories.T1Common.BustlingFungus>())
        //        .AddTile(LiquidID.Shimmer)
        //        .Register();
        //}
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

/*MULTIPLAYER BUGS
 * overloading elites crash / flicker between states
 * they also crash the client when struck and dont attach bombs
 * tesla damage is instantly healed
 * artificer weapon is not under player all the time
 * celestine circle sometimes dies instantly
 * ifrits distinction doesnt work on some players?
 * spectral circlet proj only shows up if there are npcs present
 * his reassurance doesnt work between players
 * silence two strike bombs only show up on client
 * focus crystal numbers are only purple on client
 * blazing plops dont have knockback off
 * infusion max health upgrade doesnt show for other players
 * with ukulele, enemies arent shown to take damage, and the lazer doesnt appear. the whole accessory is client side
 * war horn sucks
 * behemoth projectile doesnt rotate on spawn for other players
 * happiest mask doesnt use rollluck
 * ghosts never spawn
 * ghosts tend to vanish
 * ghosts dont get their projectiles ghosted (probably just make happiest mask a "buggy in multiplayer" item
 * other players see that those who hold headstompers have jump boost applied regardless of whether or not it is on cooldown
 * resonance disc looks like its never doing anything besides rotating for other players
 * it also disappears sometimes (maybe because theres no enemies to chase?) -TEST-
 * shattering justice doesnt make enemies different color
 * also probably isnt syncing the rornpc stat that makes it work
 * sbc souls are client side
 * forgot to shift bound soul shaders to coincide with new prefixes -TEST-
 * nvm sbc just doesnt work at all
 * artifacts dont display in chat that you activated them
 * artifact of dissonance does not work at all
 * enigma doesnt work on the other player
 * artifact of soul does not work at all
 * artifact of spite does not work at all
 * artifact of honor does not work at all
 * when you do the arti jump the other player sees you teleport
 * double tap doesnt have extra damage post penetratum -TEST-
*/