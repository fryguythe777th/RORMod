using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs
{
    public class MedkitBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] == 1)
            {
                player.Heal(10 + (int)(player.statLifeMax2 * 0.15));
            }
        }

        public override bool RightClick(int buffIndex)
        {
            return false; // Prevents removing manually
        }
    }
}