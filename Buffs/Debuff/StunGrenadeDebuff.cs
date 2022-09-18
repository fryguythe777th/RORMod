using RORMod.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Buffs.Debuff
{
    public class StunGrenadeDebuff : ModBuff
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
            npc.ROR().statDefense -= 10;
        }
    }
}