using RiskOfTerrain.UI;
using RiskOfTerrain.UI.States;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    public class BackupMagazine : ModAccessory
    {
        public static ModKeybind AmmoSwapKey { get; private set; }

        public bool hideVisual;

        public override void Load()
        {
            AmmoSwapKey = RiskOfTerrain.RegisterKeybind("Backup Magazine Swap", "MouseRight");
        }

        public override void Unload()
        {
            AmmoSwapKey = null;
        }

        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.WhiteTier.Add(Type);
            TerminalUIState.DynamicTooltip.Add(Type, () =>
            {
                return Language.GetTextValueWith("Mods.RiskOfTerrain.ItemTooltip.BackupMagazine.TerminalTooltip", new { Keybind = $"[{Helpers.GetKeyName(AmmoSwapKey)}][2:]" });
            });
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
            this.hideVisual = hideVisual;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Insert(RORItem.GetIndex(tooltips, "Consumable"), new TooltipLine(Mod, "Consumable",
                Language.GetTextValueWith("Mods.RiskOfTerrain.ItemTooltip.BackupMagazine.KeybindTooltip", new { Keybind = $"[{Helpers.GetKeyName(AmmoSwapKey)}]" })));
        }

        public override bool CanConsumeAmmo(Player player, RORPlayer ror)
        {
            return Main.rand.NextFloat(1f) < (1f - Stacks * 0.1f);
        }

        public override void ProcessTriggers(Player player, RORPlayer ror)
        {
            if (!player.mouseInterface && !player.lastMouseInterface && AmmoSwapKey.JustPressed && ModContent.GetInstance<BackupMagazineInterface>().Rotation == 0f)
            {
                ProcessAmmoSwap(player, ror);
            }
        }

        public void ProcessAmmoSwap(Player player, RORPlayer ror)
        {
            var heldItem = player.HeldItem;

            if (heldItem.useAmmo == 0)
            {
                return;
            }

            int count = 0;
            Item siftDownItem = null;
            for (int i = Main.InventoryAmmoSlotsStart; i < Main.InventoryAmmoSlotsStart + Main.InventoryAmmoSlotsCount; i++)
            {
                if (player.inventory[i].IsAir || player.inventory[i].ammo != heldItem.useAmmo)
                    continue;

                if (siftDownItem == null)
                {
                    siftDownItem = player.inventory[i];
                    continue;
                }
                count++;
                Utils.Swap(ref player.inventory[i], ref siftDownItem);
            }

            if (count == 0)
                return;

            for (int i = Main.InventoryAmmoSlotsStart; i < Main.InventoryAmmoSlotsStart + Main.InventoryAmmoSlotsCount; i++)
            {
                if (player.inventory[i].IsAir || player.inventory[i].ammo != heldItem.useAmmo)
                    continue;
                Utils.Swap(ref player.inventory[i], ref siftDownItem);
                break;
            }

            SoundEngine.PlaySound(RiskOfTerrain.GetSound("backupmagazine"), player.Center);

            ModContent.GetInstance<BackupMagazineInterface>().Opacity = 1f;
            ModContent.GetInstance<BackupMagazineInterface>().TimeActive = 0;
        }
    }
}