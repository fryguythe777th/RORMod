using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Items.Accessories;
using Microsoft.Xna.Framework;
using RiskOfTerrain.Graphics;
using RiskOfTerrain.Items.CharacterSets.Artificer;
using Terraria.Audio;
using Terraria.GameContent;
using System.Collections.Generic;

namespace RiskOfTerrain.Items.CharacterSets.Commando
{
    [AutoloadEquip(EquipType.Head)]
    public class CommandoHead : ModAccessory
    {
        public bool rollReady = true;
        public int rollCooldown = 600;

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = false;
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = false;
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
            ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = false;
            ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Insert(RORItem.GetIndex(tooltips, "BuffTime"), new TooltipLine(Mod, "BuffTime",
                Language.GetTextValueWith("Mods.RiskOfTerrain.SetBonuses.CommandoSetKeybind", new { Keybind = $"[{Helpers.GetKeyName(RORKeybinds.ArmorEffectKey)}]" })));
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 3;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<CommandoBody>() && legs.type == ModContent.ItemType<CommandoLegs>();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<RangedDamageClass>() += 0.05f;
        }

        public override void UpdateArmorSet(Player player)
        {
            if (player.ROR().commandoRollTime > 0)
            {
                player.fullRotation += MathHelper.ToRadians(10 * player.direction);
                player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                player.ROR().commandoRollTime--;

                player.controlJump = false;
                player.controlDown = false;
                player.controlLeft = false;
                player.controlRight = false;
                player.controlUp = false;
                player.controlUseItem = false;
                player.controlUseTile = false;
                player.controlThrow = false;
                player.velocity = new Vector2(player.direction * 7, player.velocity.Y);
            }

            if (rollReady && RORKeybinds.ArmorEffectKey.JustPressed)
            {
                player.ROR().commandoRollTime = 36;
                rollCooldown = 0;
                rollReady = false;
            }

            if (rollCooldown < 600)
            {
                rollCooldown++;
            }
            else
            {
                rollReady = true;
            }
        }
    }
}