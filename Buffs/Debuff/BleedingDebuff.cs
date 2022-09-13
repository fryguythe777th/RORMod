using RORMod.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Buffs.Debuff
{
    public class BleedingDebuff : ModBuff
    {
        public override string Texture => $"Terraria/Images/Buff_{BuffID.Bleeding}";

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            EnemyHealthBar.BuffIconData.Add(Type, "RORMod/Buffs/Mini/Buff_30");
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }

        public static void AddStack(NPC npc, int time, int stacksAmt)
        {
            npc.AddBuff(ModContent.BuffType<BleedingDebuff>(), time);
            npc.ROR().bleedingStacks += (byte)stacksAmt;
            npc.netUpdate = true;
        }
    }
}