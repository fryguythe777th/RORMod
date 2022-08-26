using RORMod.Buffs;
using RORMod.Buffs.Debuff;
using RORMod.Items.Accessories;
using RORMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace RORMod
{
    public class RORPlayer : ModPlayer
    {
        public bool accDeathMark;
        public bool accShatterspleen;
        public bool accMedkit;

        public override void ResetEffects()
        {
            accDeathMark = false;
            accShatterspleen = false;
            accMedkit = false;
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            if (accMedkit)
            {
                Player.AddBuff(ModContent.BuffType<MedkitBuff>(), 120);
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            OnHitEffects(target, damage, knockback, crit);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            OnHitEffects(target, damage, knockback, crit);
        }

        public void OnHitEffects(NPC target, int damage, float knockback, bool crit)
        {
            if (accDeathMark)
            {
                int buffCount = 0;
                for (int i = 0; i < NPC.maxBuffs; i++)
                {
                    if (target.buffType[i] != 0 && Main.debuff[target.buffType[i]])
                    {
                        buffCount++;
                    }
                    if (buffCount >= 2)
                    {
                        target.AddBuff(ModContent.BuffType<DeathMarkDebuff>(), 420);
                        break;
                    }
                }
            }
            if (accShatterspleen && crit)
            {
                target.GetGlobalNPC<RORNPC>().bleedShatterspleen = true;
                BleedingDebuff.AddStack(target, 300, 1);
                target.netUpdate = true;
            }
        }
    }
}