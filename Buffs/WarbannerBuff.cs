using Terraria;
using Terraria.ModLoader;

namespace RORMod.Buffs
{
    public class WarbannerBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Generic) += 0.33f;
            player.moveSpeed += 0.6f;
            player.ROR().bootSpeed += 3.33f;
        }

        public override bool RightClick(int buffIndex)
        {
            return false; // Prevents removing manually
        }
    }
}