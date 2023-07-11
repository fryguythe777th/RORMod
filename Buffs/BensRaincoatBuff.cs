using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs
{
    public class BensRaincoatBuff : ModBuff
    {
        public override bool RightClick(int buffIndex)
        {
            return false; // Prevents removing manually
        }
    }
}