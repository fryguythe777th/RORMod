using Microsoft.Xna.Framework;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Common;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Content.Elites;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using RiskOfTerrain.Tiles.Furniture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Diagnostics;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Generation;
using Terraria.Social;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using RiskOfTerrain.Items.Accessories.T2Uncommon;
using RiskOfTerrain.Items.Accessories.T3Legendary;

namespace RiskOfTerrain.Items
{
    public class RORItem : GlobalItem
    {
        public static List<ChestDropInfo> WhiteTier { get; private set; }
        public static List<ChestDropInfo> GreenTier { get; private set; }
        public static List<ChestDropInfo> RedTier { get; private set; }
        public static List<ChestDropInfo> BossTier { get; private set; }
        public static List<ChestDropInfo> LunarTier { get; private set; }
        public static List<ChestDropInfo> VoidTier { get; private set; }
        public static List<ChestDropInfo> Equipment { get; private set; }

        internal static readonly string[] TooltipNames = new string[]
        {
                "ItemName",
                "Favorite",
                "FavoriteDesc",
                "Social",
                "SocialDesc",
                "Damage",
                "CritChance",
                "Speed",
                "Knockback",
                "FishingPower",
                "NeedsBait",
                "BaitPower",
                "Equipable",
                "WandConsumes",
                "Quest",
                "Vanity",
                "Defense",
                "PickPower",
                "AxePower",
                "HammerPower",
                "TileBoost",
                "HealLife",
                "HealMana",
                "UseMana",
                "Placeable",
                "Ammo",
                "Consumable",
                "Material",
                "Tooltip#",
                "EtherianManaWarning",
                "WellFedExpert",
                "BuffTime",
                "OneDropLogo",
                "PrefixDamage",
                "PrefixSpeed",
                "PrefixCritChance",
                "PrefixUseMana",
                "PrefixSize",
                "PrefixShootSpeed",
                "PrefixKnockback",
                "PrefixAccDefense",
                "PrefixAccMaxMana",
                "PrefixAccCritChance",
                "PrefixAccDamage",
                "PrefixAccMoveSpeed",
                "PrefixAccMeleeSpeed",
                "SetBonus",
                "Expert",
                "Master",
                "JourneyResearch",
                "BestiaryNotes",
                "SpecialPrice",
                "Price",
        };

        public override void Load()
        {
            WhiteTier = new List<ChestDropInfo>();
            GreenTier = new List<ChestDropInfo>();
            RedTier = new List<ChestDropInfo>();
            BossTier = new List<ChestDropInfo>();
            LunarTier = new List<ChestDropInfo>();
            VoidTier = new List<ChestDropInfo>();
            Equipment = new List<ChestDropInfo>();
        }

        public override void Unload()
        {
            WhiteTier?.Clear();
            WhiteTier = null;
            GreenTier?.Clear();
            GreenTier = null;
            RedTier?.Clear();
            RedTier = null;
            BossTier?.Clear();
            BossTier = null;
            LunarTier?.Clear();
            LunarTier = null;
            VoidTier?.Clear();
            VoidTier = null;
            Equipment?.Clear();
            Equipment = null;
        }

        public override void OnSpawn(Item item, IEntitySource source)
        {
            if (source is EntitySource_Loot death)
            {
                if (death.Entity is NPC npc && npc.GetGlobalNPC<GhostElite>().Active)
                {
                    item.TurnToAir();
                }
            }
        }

        public static int GetIndex(List<TooltipLine> tooltips, string lineName)
        {
            int myIndex = FindLineIndex(lineName);
            int i = 0;
            for (; i < tooltips.Count; i++)
            {
                if (tooltips[i].Mod == "Terraria" && FindLineIndex(tooltips[i].Name) >= myIndex)
                {
                    if (lineName == "Tooltip#")
                    {
                        int finalIndex = i;
                        while (i < tooltips.Count)
                        {
                            if (tooltips[i].Name.StartsWith("Tooltip"))
                            {
                                finalIndex = i;
                            }
                            i++;
                        }
                        return finalIndex;
                    }
                    return i;
                }
            }
            return i;
        }

        private static int FindLineIndex(string name)
        {
            if (name.StartsWith("Tooltip"))
            {
                name = "Tooltip#";
            }
            for (int i = 0; i < TooltipNames.Length; i++)
            {
                if (name == TooltipNames[i])
                {
                    return i;
                }
            }
            return -1;
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            player.ROR().Accessories.AddItemStack(item);
        }

        private bool? UseCheckLock(Item item, Player player) {
            var tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            if (tile.TileFrameX >= 36 || TileLoader.GetTile(tile.TileType) is not SecurityChestTile) {
                return null;
            }

            int left = Player.tileTargetX - tile.TileFrameX / 18;
            int top = Player.tileTargetY - tile.TileFrameY / 18;
            //Dust.NewDustPerfect(new Vector2(left * 16f, top * 16f), DustID.Torch).noGravity = true;
            SoundEngine.PlaySound(SoundID.Unlock);
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    Main.tile[left + i, top + j].TileFrameX += 36;
                }
            }
            return true;
        }

        public override bool? UseItem(Item item, Player player)
        {
            player.ROR().Accessories.OnUseItem(player, item);

            if (player.whoAmI == Main.myPlayer && player.IsInTileInteractionRange(Player.tileTargetX, Player.tileTargetY, TileReachCheckSettings.Simple)) {
                if (item.type == ItemID.ChestLock) {
                    return UseCheckLock(item, player);
                }
            }
            return null;
        }
    }

    public class RORItemSystem : ModSystem
    {
        public override void AddRecipeGroups()
        {
            RecipeGroup copperOrTin = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CopperBar)}", ItemID.CopperBar, ItemID.TinBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.CopperBar), copperOrTin);
        }

        public static Item AddItem(Chest chest, int item, int stack = 1, int prefix = 0)
        {
            Item emptySlot = null;
            for (int i = 0; i < Chest.maxItems; i++)
            {
                if (chest.item[i].IsAir && emptySlot == null)
                {
                    emptySlot = chest.item[i];
                }
            }

            if (emptySlot != null)
            {
                emptySlot.SetDefaults(item);
                emptySlot.stack = stack;
                if (prefix > 0 || prefix < 0)
                {
                    emptySlot.Prefix(prefix);
                }
            }
            return emptySlot;
        }

        public override void PostWorldGen()
        {
            var r = WorldGen.genRand;

            for (int k = 0; k < Main.maxChests; k++)
            {
                Chest c = Main.chest[k];

                if (c != null && WorldGen.InWorld(c.x, c.y, 40))
                {
                    var tile = Main.tile[c.x, c.y];
                    var wall = tile.WallType;
                    if (wall == WallID.SandstoneBrick)
                    {
                        continue;
                    }

                    int style = Main.tile[c.x, c.y].TileFrameX / 36;

                    if (Main.tile[c.x, c.y].TileType == TileID.Containers)
                    {
                        if (style == 0)
                        {
                            if (r.NextBool(10))
                            {
                                if (r.NextBool())
                                {
                                    AddItem(c, ModContent.ItemType<OldGuillotine>(), prefix: -1);
                                }
                                else 
                                {
                                    AddItem(c, ModContent.ItemType<OldWarStealthkit>(), prefix: -1);
                                }
                            }
                        }
                        if (style == 2)
                        {
                            if (r.NextBool(7))
                            {
                                AddItem(c, ModContent.ItemType<ReloadingShurikens>(), prefix: -1);
                            }
                        }
                        if (style == 50)
                        {
                            if (r.NextBool(2))
                            {
                                AddItem(c, ModContent.ItemType<ShatteringJustice>(), prefix: -1);
                            }
                        }
                        else if (style == 51)
                        {
                            if (r.NextBool(2))
                            {
                                AddItem(c, ModContent.ItemType<Aegis>(), prefix: -1);
                            }
                        }
                    }
                }
            }
        }
    }
}
