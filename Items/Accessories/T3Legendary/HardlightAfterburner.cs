using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.NPCs;
using Terraria.Audio;
using RiskOfTerrain.Buffs;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class HardlightAfterburner : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add((Type, () => NPC.downedMechBossAny));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public int dashRechargeTime;
        public int dashCharges;
        public bool currentlyDashing;
        public Vector2 savedVelocity;
        public int dashingCounter = 10;

        public override void OnUnequip(EntityInfo entity)
        {
            entity.ClearBuff(ModContent.BuffType<AfterburnerBuff>());
            dashCharges = 0;
            dashRechargeTime = 0;
        }

        public override void OnEquip(EntityInfo entity)
        {
            entity.AddInfoBuff<AfterburnerBuff>();
            dashCharges = 0;
            dashRechargeTime = 0;
            if (entity.IsMe())
            {
                AfterburnerBuff.Brightness = 0f;
                AfterburnerBuff.Charges = 0;
                AfterburnerBuff.AtMaxCharge = true;
                AfterburnerBuff.RechargePercentage = 0f;
            }
        }

        public override void PostUpdateEquips(EntityInfo entity)
        {
            if (dashCharges != 0)
            {
                entity.AddInfoBuff<AfterburnerBuff>();
            }
            if (dashCharges < 3)
            {
                dashRechargeTime++;
                if (dashRechargeTime > 600)
                {
                    if (entity.IsMe())
                    {
                        ShurikenBuff.Brightness = 0.75f;
                    }
                    dashRechargeTime = 0;
                    dashCharges++;
                } 
            }
            if (entity.IsMe())
            {
                AfterburnerBuff.Brightness *= 0.95f;
                AfterburnerBuff.RechargePercentage = dashRechargeTime / 600f;
                AfterburnerBuff.AtMaxCharge = dashCharges >= 3;
                AfterburnerBuff.Charges = dashCharges;
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (dashCharges > 0 && RORKeybinds.AfterburnerDashKey.JustPressed && !currentlyDashing)
            {
                dashCharges--;
                dashRechargeTime = 0;
                if (dashCharges <= 0)
                {
                    player.ClearBuff(ModContent.BuffType<AfterburnerBuff>());
                }

                currentlyDashing = true;
                player.eocDash += 20;
                savedVelocity = player.velocity;
            }

            if (currentlyDashing)
            {
                Vector2 distanceToMouse = player.Center - Main.MouseWorld;
                float rotToMouse = distanceToMouse.ToRotation();
                player.velocity = new Vector2(0, 40).RotatedBy(rotToMouse + MathHelper.PiOver2);

                if (dashingCounter == 0)
                {
                    dashingCounter = 10;
                    player.velocity = savedVelocity;
                    currentlyDashing = false;
                    player.eocDash -= 20;
                }
                else
                {
                    dashingCounter--;
                }
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Insert(RORItem.GetIndex(tooltips, "Consumable"), new TooltipLine(Mod, "Consumable",
                Language.GetTextValueWith("Mods.RiskOfTerrain.Items.HardlightAfterburner.KeybindTooltip", new { Keybind = $"[{Helpers.GetKeyName(RORKeybinds.AfterburnerDashKey)}]" })));
        }
    }
}