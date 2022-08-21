using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROR2Artifacts
{
    public class ArtifactSystem : ModSystem
    {
        public static int DeathCooldown;

        public override void OnWorldLoad()
        {
            ROR2Artifacts.ChaosActive = false;
            ROR2Artifacts.CommandActive = false;
            ROR2Artifacts.DeathActive = false;
            ROR2Artifacts.DissonanceActive = false;
            ROR2Artifacts.EnigmaActive = false;
            ROR2Artifacts.EvolutionActive = false;
            ROR2Artifacts.FrailtyActive = false;
            ROR2Artifacts.GlassActive = false;
            ROR2Artifacts.HonorActive = false;
            ROR2Artifacts.KinActive = false;
            ROR2Artifacts.MetamorphosisActive = false;
            ROR2Artifacts.SacrificeActive = false;
            ROR2Artifacts.SoulActive = false;
            ROR2Artifacts.SpiteActive = false;
            ROR2Artifacts.SwarmsActive = false;
            ROR2Artifacts.VenganceActive = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            var l = new List<string>();
            Save(ROR2Artifacts.ChaosActive, "Chaos", l);
            Save(ROR2Artifacts.CommandActive, "Command", l);
            Save(ROR2Artifacts.DeathActive, "Death", l);
            Save(ROR2Artifacts.DissonanceActive, "Dissonance", l);
            Save(ROR2Artifacts.EnigmaActive, "Enigma", l);
            Save(ROR2Artifacts.EvolutionActive, "Evolution", l);
            Save(ROR2Artifacts.FrailtyActive, "Frailty", l);
            Save(ROR2Artifacts.GlassActive, "Glass", l);
            Save(ROR2Artifacts.HonorActive, "Honor", l);
            Save(ROR2Artifacts.KinActive, "Kin", l);
            Save(ROR2Artifacts.MetamorphosisActive, "Metamorphosis", l);
            Save(ROR2Artifacts.SacrificeActive, "Sacrifice", l);
            Save(ROR2Artifacts.SoulActive, "Soul", l);
            Save(ROR2Artifacts.SpiteActive, "Spite", l);
            Save(ROR2Artifacts.SwarmsActive, "Swarms", l);
            Save(ROR2Artifacts.VenganceActive, "Vengance", l);
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
            ROR2Artifacts.ChaosActive = l.Contains("Chaos");
            ROR2Artifacts.CommandActive = l.Contains("Command");
            ROR2Artifacts.DeathActive = l.Contains("Death");
            ROR2Artifacts.DissonanceActive = l.Contains("Dissonance");
            ROR2Artifacts.EnigmaActive = l.Contains("Enigma");
            ROR2Artifacts.EvolutionActive = l.Contains("Evolution");
            ROR2Artifacts.FrailtyActive = l.Contains("Frailty");
            ROR2Artifacts.GlassActive = l.Contains("Glass");
            ROR2Artifacts.HonorActive = l.Contains("Honor");
            ROR2Artifacts.KinActive = l.Contains("Kin");
            ROR2Artifacts.MetamorphosisActive = l.Contains("Metamorphosis");
            ROR2Artifacts.SacrificeActive = l.Contains("Sacrifice");
            ROR2Artifacts.SoulActive = l.Contains("Soul");
            ROR2Artifacts.SpiteActive = l.Contains("Spite");
            ROR2Artifacts.SwarmsActive = l.Contains("Swarms");
            ROR2Artifacts.VenganceActive = l.Contains("Vengance");
        }

        public override void PreUpdatePlayers()
        {
            if (!ROR2Artifacts.DeathActive)
            {
                return;
            }

            if (DeathCooldown > 0)
            {
                DeathCooldown--;
                return;
            }

            int death = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && Main.player[i].dead)
                {
                    death = i;
                }
            }

            if (death > -1)
            {
                DeathCooldown = 240;
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active && !Main.player[i].dead)
                    {
                        Main.player[i].KillMe(PlayerDeathReason.ByCustomReason(Language.GetTextValue("Mods.ROR2Artifacts.DeathReason.DeathArtifact", Main.player[i].name, Main.player[death].name)), Main.player[i].statLifeMax2 * 2, -Main.player[i].direction);
                    }
                }
            }
        }

        public override void PostUpdateNPCs()
        {
            if (ROR2Artifacts.DissonanceActive && Main.netMode != NetmodeID.Server)
            {
                Main.LocalPlayer.UpdateBiomes();
            }
        }
    }
}