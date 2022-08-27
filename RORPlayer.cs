using RORMod.Buffs;
using RORMod.Buffs.Debuff;
using RORMod.Items.Accessories;
using RORMod.NPCs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace RORMod
{
    public class RORPlayer : ModPlayer
    {
        public bool accDeathMark;
        public bool accShatterspleen;
        public bool accMedkit;
        public bool accTougherTimes;
        public bool accMonsterTooth;

        public override void ResetEffects()
        {
            accDeathMark = false;
            accShatterspleen = false;
            accMedkit = false;
            accTougherTimes = false;
            accMonsterTooth = false;
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (accTougherTimes && Main.rand.NextBool(10))
            {
                Player.NinjaDodge();
                return false;
            }
            return true;
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