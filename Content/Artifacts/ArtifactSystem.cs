using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace RiskOfTerrain.Content.Artifacts
{
    public class ArtifactSystem : ModSystem
    {
        public static bool chaos;
        public static bool command;
        public static bool death;
        public static int deathCooldown;
        public static bool dissonance;
        public static bool enigma;
        public static bool evolution;
        public static bool frailty;
        public static bool glass;
        public static bool honor;
        public static bool kin;
        public static bool metamorphosis;
        public static bool sacrifice;
        public static bool soul;
        public static bool spite;
        public static bool swarms;
        public static bool vengeance;

        public override void OnWorldLoad()
        {
            chaos = false;
            command = false;
            death = false;
            dissonance = false;
            enigma = false;
            evolution = false;
            frailty = false;
            glass = false;
            honor = false;
            kin = false;
            metamorphosis = false;
            sacrifice = false;
            soul = false;
            spite = false;
            swarms = false;
            vengeance = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            var l = new List<string>();
            Save(chaos, "Chaos", l);
            Save(command, "Command", l);
            Save(death, "Death", l);
            Save(dissonance, "Dissonance", l);
            Save(enigma, "Enigma", l);
            Save(evolution, "Evolution", l);
            Save(frailty, "Frailty", l);
            Save(glass, "Glass", l);
            Save(honor, "Honor", l);
            Save(kin, "Kin", l);
            Save(metamorphosis, "Metamorphosis", l);
            Save(sacrifice, "Sacrifice", l);
            Save(soul, "Soul", l);
            Save(spite, "Spite", l);
            Save(swarms, "Swarms", l);
            Save(vengeance, "Vengeance", l);
            tag["Artifacts"] = l;
        }
        private void Save(bool b, string t, List<string> l)
        {
            if (b)
                l.Add(t);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            var l = tag.Get<List<string>>("Artifacts");
            chaos = l.Contains("Chaos");
            command = l.Contains("Command");
            death = l.Contains("Death");
            dissonance = l.Contains("Dissonance");
            enigma = l.Contains("Enigma");
            evolution = l.Contains("Evolution");
            frailty = l.Contains("Frailty");
            glass = l.Contains("Glass");
            honor = l.Contains("Honor");
            kin = l.Contains("Kin");
            metamorphosis = l.Contains("Metamorphosis");
            sacrifice = l.Contains("Sacrifice");
            soul = l.Contains("Soul");
            spite = l.Contains("Spite");
            swarms = l.Contains("Swarms");
            vengeance = l.Contains("Vengance");
        }

        public override void PreUpdatePlayers()
        {
            CheckDeath();
        }
        public static void CheckDeath()
        {
            if (!death)
            {
                return;
            }

            if (deathCooldown > 0)
            {
                deathCooldown--;
                return;
            }

            int deathPlayer = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && Main.player[i].dead)
                {
                    deathPlayer = i;
                }
            }

            if (deathPlayer > -1)
            {
                deathCooldown = 240;
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active && !Main.player[i].dead)
                    {
                        Main.player[i].KillMe(PlayerDeathReason.ByCustomReason(Language.GetTextValue("Mods.RiskOfTerrain.DeathReason.DeathArtifact", Main.player[i].name, Main.player[deathPlayer].name)), Main.player[i].statLifeMax2 * 2, -Main.player[i].direction);
                    }
                }
            }
        }

        public override void PostUpdateNPCs()
        {
            if (dissonance && Main.netMode != NetmodeID.Server)
            {
                Main.LocalPlayer.UpdateBiomes();
            }
        }
    }
}