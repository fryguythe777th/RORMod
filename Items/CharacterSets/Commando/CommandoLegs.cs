using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RiskOfTerrain.Graphics;
using RiskOfTerrain.Items.Accessories.T3Legendary;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Commando
{
    [AutoloadEquip(EquipType.Legs)]
    public class CommandoLegs : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Legs.Sets.HidesTopSkin[Item.legSlot] = true;
            ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Insert(RORItem.GetIndex(tooltips, "BuffTime"), new TooltipLine(Mod, "BuffTime",
                Language.GetTextValueWith("Mods.RiskOfTerrain.SetBonuses.CommandoSetKeybind", new { Keybind = $"[{Helpers.GetKeyName(RORKeybinds.ArmorEffectKey)}]" })));
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 14;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<RangedDamageClass>().Flat += 1;
        }
    }
}