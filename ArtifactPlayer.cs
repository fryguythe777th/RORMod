using ROR2Artifacts.Items.Artifacts;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ROR2Artifacts.Items.Accessories;

namespace ROR2Artifacts
{
    public class ArtifactPlayer : ModPlayer
    {
        public int enigmaDelay;
        public bool DeathMark;

        public override void ResetEffects()
        {
            DeathMark = false;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (DeathMark)
            {
                int buffCount = 0;
                for (int i = 0; i < NPC.maxBuffs; i++)
                {
                    if (target.buffType[i] != 0)
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
        }

        public override bool PreItemCheck()
        {
            if (ROR2Artifacts.EnigmaActive)
            {
                if (enigmaDelay <= 0)
                {
                    Player.channel = false;
                    if (Player.ItemTimeIsZero && Main.myPlayer == Player.whoAmI)
                    {
                        enigmaDelay = 180;
                        for (int i = 0; i < 10; i++)
                        {
                            Utils.Swap(ref Player.inventory[i], ref Player.inventory[Main.rand.Next(10)]);
                        }
                        if (Player.inventory[58] != null && !Player.inventory[58].IsAir)
                        {
                            Utils.Swap(ref Player.inventory[58], ref Player.inventory[Main.rand.Next(10)]);
                            Main.mouseItem = Player.inventory[58];
                        }
                    }
                }
            }
            if (enigmaDelay > 0 && Player.HeldItem.type != ModContent.ItemType<Enigma>())
                enigmaDelay--;
            return true;
        }

        public override bool? CanAutoReuseItem(Item item)
        {
            if (ROR2Artifacts.EnigmaActive && enigmaDelay <= 0)
            {
                return false;
            }
            return null;
        }

        public override void PostUpdateEquips()
        {
            if (ROR2Artifacts.FrailtyActive)
            {
                if ((int)Player.position.Y / 16 - Player.fallStart > 10)
                {
                    Player.equippedWings = null;
                    Player.wingsLogic = 0;
                    Player.noFallDmg = true;
                }
                if (!Player.noFallDmg)
                {
                    Player.extraFall /= 2;
                    Player.extraFall -= 15;
                }
                else
                {
                    Player.noFallDmg = false;
                }
            }
            if (ROR2Artifacts.GlassActive)
            {
                Player.statLifeMax2 /= 10;
                Player.GetDamage<GenericDamageClass>() *= 5f;
            }
        }

        public override void PostUpdate()
        {
            if (ROR2Artifacts.ChaosActive)
            {
                Player.hostile = true;
                Player.team = Main.myPlayer == Player.whoAmI ? 0 : 1;
            }
            if (ROR2Artifacts.DissonanceActive)
            {
                bool inPillars = Player.ZoneTowerNebula || Player.ZoneTowerSolar || Player.ZoneTowerStardust || Player.ZoneTowerVortex;
                Player.zone1 = (byte)Main.rand.Next(byte.MaxValue);
                Player.zone2 = (byte)Main.rand.Next(byte.MaxValue);
                Player.zone3 = (byte)Main.rand.Next(byte.MaxValue);
                Player.zone4 = (byte)Main.rand.Next(byte.MaxValue);
                Player.ZoneWaterCandle = false;
                Player.ZonePeaceCandle = false;
                Player.ZoneOldOneArmy = false;
                Player.ZoneDungeon = !NPC.downedBoss3 ? false : Main.rand.NextBool(7);
                Player.ZoneMeteor = !NPC.downedBoss2 ? false : Main.rand.NextBool(35);
                Player.ZoneTowerNebula = false;
                Player.ZoneTowerSolar = false;
                Player.ZoneTowerStardust = false;
                Player.ZoneTowerVortex = false;
                if (inPillars)
                {
                    switch (Main.rand.Next(4))
                    {
                        case 0:
                            Player.ZoneTowerNebula = true;
                            break;
                        case 1:
                            Player.ZoneTowerSolar = true;
                            break;
                        case 2:
                            Player.ZoneTowerStardust = true;
                            break;
                        case 3:
                            Player.ZoneTowerVortex = true;
                            break;
                    }
                }
            }
        }
    }
}