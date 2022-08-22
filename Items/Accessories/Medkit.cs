using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace ROR2Artifacts.Items.Accessories
{
    public class Medkit : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Taking damage will give you an Automatic Healing buff for 7 seconds\n" +
                "When the buff runs out, the player will heal 10 HP + 5% of their max HP\n" +
                "Taking damage again will reset the timer");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.White;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ArtifactPlayer artifactPlayer = player.GetModPlayer<ArtifactPlayer>();
            artifactPlayer.Medkit = true;
        }
    }

    public class AutomaticHealingBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Automatic Healing Buff");
            Description.SetDefault("Heal a small amount of health when this buff runs out");
            Main.buffNoSave[this.Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] == 1)
            {
                player.Heal(10 + (int)(player.statLifeMax * 0.05));
            }
        }
    }
}