using RORMod.Items.Artifacts;
using Terraria;
using Terraria.ModLoader;

namespace RORMod.Content.Artifacts
{
    public class ArtifactPlayer : ModPlayer
    {
        public int enigmaDelay;

        public override bool PreItemCheck()
        {
            if (ArtifactSystem.enigma)
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
            return ArtifactSystem.enigma && enigmaDelay <= 0 ? false : null;
        }

        public override void PostUpdateEquips()
        {
            if (ArtifactSystem.frailty)
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
            if (ArtifactSystem.glass)
            {
                Player.statLifeMax2 /= 10;
                Player.GetDamage<GenericDamageClass>() *= 5f;
            }
        }

        public override void PostUpdate()
        {
            if (ArtifactSystem.chaos)
            {
                Player.hostile = true;
                Player.team = Main.myPlayer == Player.whoAmI ? 0 : 1;
            }
            if (ArtifactSystem.dissonance)
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