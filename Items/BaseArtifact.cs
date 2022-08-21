using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ROR2Artifacts.Items
{
    public abstract class BaseArtifact : ModItem
    {
        public abstract bool ActiveFlag { get; set; }

        public virtual bool unimplemented => false;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 50);
        }

        public override bool? UseItem(Player player)
        {
            ActiveFlag = !ActiveFlag;
            if (Main.myPlayer == player.whoAmI)
                ROR2Artifacts.BroadcastKeys("Announcements.Artifact" + (ActiveFlag ? "Enabled" : "Disabled"), ROR2Artifacts.BossSummonMessage, "Mods.ROR2Artifacts.ItemName." + Name, player.name);
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (unimplemented)
                tooltips.Add(new TooltipLine(Mod, "Unimplemented", Language.GetTextValue("Mods.ROR2Artifacts.UnimplementedItem")) { OverrideColor = Color.Gray });
        }
    }
}