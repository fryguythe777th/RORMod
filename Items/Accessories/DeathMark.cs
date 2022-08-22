using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace ROR2Artifacts.Items.Accessories
{
    public class DeathMark : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Damaging enemies with 2 or more debuffs will mark them for death for 7 seconds\n" +
                "Enemies marked for death will take 15% more damage");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ArtifactPlayer artifactPlayer = player.GetModPlayer<ArtifactPlayer>();
            artifactPlayer.DeathMark = true;
        }
    }

    public class DeathMarkDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Description.SetDefault("This enemy will take 15% more damage");
            Main.buffNoSave[this.Type] = true;
            Main.buffNoTimeDisplay[this.Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<ArtifactNPC>().deathMarkDamageIncrease = true;
        }
    }
}