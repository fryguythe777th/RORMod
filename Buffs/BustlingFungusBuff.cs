using Terraria;
using Terraria.ModLoader;

namespace RORMod.Buffs
{
    public class BustlingFungusBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override bool RightClick(int buffIndex)
        {
            return false; // Prevents removing manually
        }
    }
}