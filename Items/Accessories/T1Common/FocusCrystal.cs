using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    public class FocusCrystal : ModAccessory, ItemHooks.IUpdateItemDye
    {
        public static Entity HitNPCForMakingDamageNumberPurpleHack;
        public static Color HitColor;
        public static Color HitColorCrit;

        public override void Load()
        {
            HitColor = new Color(244, 105, 200, 255);
            HitColorCrit = HitColor * 1.4f;
            On.Terraria.CombatText.NewText_Rectangle_Color_string_bool_bool += CombatText_NewText_Rectangle_Color_string_bool_bool;
        }

        private static int CombatText_NewText_Rectangle_Color_string_bool_bool(On.Terraria.CombatText.orig_NewText_Rectangle_Color_string_bool_bool orig, Rectangle location, Color color, string text, bool dramatic, bool dot)
        {
            if (HitNPCForMakingDamageNumberPurpleHack != null && location.X == (int)HitNPCForMakingDamageNumberPurpleHack.position.X && location.Y == (int)HitNPCForMakingDamageNumberPurpleHack.position.Y)
            {
                if (color == CombatText.DamagedHostileCrit || color == CombatText.DamagedFriendlyCrit)
                {
                    color = HitColorCrit;
                }
                else
                {
                    color = HitColor;
                }
                HitNPCForMakingDamageNumberPurpleHack = null;
            }
            return orig(location, color, text, dramatic, dot);
        }

        public override void Unload()
        {
            HitNPCForMakingDamageNumberPurpleHack = null;
        }

        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.WhiteTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var ror = player.ROR();
            ror.accFocusCrystal = Item;
            ror.focusCrystalDiameter = Math.Max(ror.focusCrystalDiameter, 480f);
            ror.focusCrystalVisible = !hideVisual;
            ror.focusCrystalDamage += 0.25f;
        }

        void ItemHooks.IUpdateItemDye.UpdateItemDye(Player player, bool isNotInVanitySlot, bool isSetToHidden, Item armorItem, Item dyeItem)
        {
            player.ROR().cFocusCrystal = dyeItem.dye;
        }

        public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref int damage, ref float knockBack, ref bool crit)
        {
            if (entity.entity.Distance(victim.entity.Hitbox.ClosestDistance(entity.entity.Center)) < 240f)
            {
                damage = (int)(damage * (1f + 0.25f * Item.stack));
                HitNPCForMakingDamageNumberPurpleHack = victim.entity;
            }
        }
    }
}
