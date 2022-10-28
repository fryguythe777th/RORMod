using RiskOfTerrain.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs.Debuff
{
    public class BleedingDebuff : ModBuff
    {
        public override string Texture => $"Terraria/Images/Buff_{BuffID.Bleeding}";

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            EnemyHealthBar.BuffIconData.Add(Type, "RiskOfTerrain/Buffs/Mini/Buff_30");
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }

        public static void AddStack(Entity entity, int time, int stacksAmt)
        {
            if (entity is NPC npc)
            {
                npc.AddBuff(ModContent.BuffType<BleedingDebuff>(), time);
                npc.ROR().bleedingStacks += (byte)stacksAmt;
                npc.netUpdate = true;
            }
            else if (entity is Player player)
            {
                player.AddBuff(BuffID.Bleeding, time * stacksAmt);
            }
        }
    }
}