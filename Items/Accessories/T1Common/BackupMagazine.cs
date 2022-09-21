using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RORMod.Items.Accessories.T1Common
{
    public class BackupMagazine : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.WhiteTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var ror = player.ROR();
            ror.accBackupMagazine = true;
            ror.backupMagVisible = !hideVisual;
            ror.backupMagAmmoReduction += 0.1f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Insert(RORItem.GetIndex(tooltips, "Consumable"), new TooltipLine(Mod, "Consumable", Language.GetTextValueWith("Mods.RORMod.ItemTooltip.BackupMagazine.KeybindTooltip", new { Keybind = $"[{Helpers.GetKeyName(RORPlayer.AmmoSwapKey)}]" })));
        }
    }
}