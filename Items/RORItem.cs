using Microsoft.Xna.Framework;
using RORMod.Buffs;
using RORMod.Projectiles.Misc;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace RORMod.Items
{
    public class RORItem : GlobalItem
    {
        public static List<int> WhiteTier { get; private set; }
        public static List<int> GreenTier { get; private set; }
        public static List<int> RedTier { get; private set; }
        public static List<int> BossTier { get; private set; }
        public static List<int> LunarTier { get; private set; }
        public static List<int> VoidTier { get; private set; }

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
            WhiteTier = new List<int>();
            GreenTier = new List<int>();
            RedTier = new List<int>();
            BossTier = new List<int>();
            LunarTier = new List<int>();
            VoidTier = new List<int>();
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

        public override bool? UseItem(Item item, Player player)
        {
            var ror = player.ROR();
            if (Main.myPlayer == player.whoAmI && !player.HeldItem.IsAir && player.HeldItem.damage > 0 && ror.accShuriken != null && ror.shurikenCharges > 0 && (ror.shurikenRechargeTime > 20 || ror.shurikenCharges == ror.shurikenChargesMax))
            {
                ror.shurikenCharges--;
                ror.shurikenRechargeTime = 0;
                if (ror.shurikenCharges <= 0)
                {
                    player.ClearBuff(ModContent.BuffType<ShurikenBuff>());
                }
                var p = Projectile.NewProjectileDirect(player.GetSource_Accessory(ror.accShuriken), player.Center, Vector2.Normalize(Main.MouseWorld - player.Center) * 20f, 
                    ModContent.ProjectileType<ReloadingShurikenProj>(), Math.Clamp(player.GetWeaponDamage(player.HeldItem) * 2, 10, 200), 1f, player.whoAmI);
                p.DamageType = player.HeldItem.DamageType;
            }
            return null;
        }
    }
}