using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Commando
{
    [AutoloadEquip(EquipType.Body)]
    public class CommandoBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Insert(RORItem.GetIndex(tooltips, "BuffTime"), new TooltipLine(Mod, "BuffTime",
                Language.GetTextValueWith("Mods.RiskOfTerrain.SetBonuses.CommandoSetKeybind", new { Keybind = $"[{Helpers.GetKeyName(RORKeybinds.ArmorEffectKey)}]" })));
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat(1f) < 0.95f;
        }
    }
}