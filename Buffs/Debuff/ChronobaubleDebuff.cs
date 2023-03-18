using RiskOfTerrain.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs.Debuff
{
    public class ChronobaubleDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.maxRunSpeed *= 0.66f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.ROR().npcSpeedStat *= 0.5f;
        }
    }
}