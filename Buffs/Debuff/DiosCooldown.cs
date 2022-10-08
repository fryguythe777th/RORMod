using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs.Debuff
{
    public class DiosCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = player.ROR().diosCooldown;
        }
    }
}