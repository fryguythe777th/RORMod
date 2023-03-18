using RiskOfTerrain.Items.Accessories.T1Common;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs
{
    public class CelestineInvis : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }

    public class CelestineSlow : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.maxRunSpeed *= 0.2f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.ROR().npcSpeedStat *= 0.2f;
        }
    }
}