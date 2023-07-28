using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Artificer
{
    [AutoloadEquip(EquipType.Body)]
    public class ArtificerBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 30;
            if (player.controlJump && Math.Sign(player.velocity.Y) == 1 && player.mount._type == MountID.None && !player.pulley)
            {
                player.velocity.Y = MathHelper.Lerp(player.velocity.Y, 0.1f, 0.40f);
            }
        }
    }
}