using RORMod.Buffs;
using RORMod.Buffs.Debuff;
using RORMod.Items.Accessories;
using RORMod.NPCs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Localization;

namespace RORMod
{
    public class RORPlayer : ModPlayer
    {
        public bool accDeathMark;
        public bool accShatterspleen;
        public bool accMedkit;
        public bool accTougherTimes;
        public bool accMonsterTooth;
        public bool accTriTipDagger;

        public override void ResetEffects()
        {
            accDeathMark = false;
            accShatterspleen = false;
            accMedkit = false;
            accTougherTimes = false;
            accMonsterTooth = false;
            accTriTipDagger = false;
        }

        public void TougherTimesDodge()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                SoundEngine.PlaySound(RORMod.GetSound("toughertimes").WithVolumeScale(0.2f), Player.Center);
                int c = CombatText.NewText(new Rectangle((int)Player.position.X + Player.width / 2 - 1, (int)Player.position.Y, 2, 2), new Color(255, 255, 255, 255), 0, false, true);
                if (c != -1 && c != Main.maxCombatText)
                {
                    Main.combatText[c].text = Language.GetTextValue("Mods.RORMod.Blocked");
                    Main.combatText[c].rotation = 0f;
                    Main.combatText[c].scale *= 0.8f;
                    Main.combatText[c].alphaDir = 0;
                    Main.combatText[c].alpha = 0.99f;
                    Main.combatText[c].position.X = Player.position.X + Player.width / 2f - FontAssets.CombatText[0].Value.MeasureString(Main.combatText[c].text).X / 2f;
                }
            }
            Player.SetImmuneTimeForAllTypes(60);
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (Player.whoAmI == Main.myPlayer && accTougherTimes && Main.rand.NextBool(10))
            {
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    var p = RORMod.GetPacket(PacketType.TougherTimesDodge);
                    p.Write(Player.whoAmI);
                }
                TougherTimesDodge();
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
            if (accTriTipDagger && Main.rand.NextBool(10))
            {
                BleedingDebuff.AddStack(target, 180, 1);
            }
        }
    }
}