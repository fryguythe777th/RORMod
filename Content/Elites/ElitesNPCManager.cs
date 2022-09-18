using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RORMod.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Content.Elites
{
    public class ElitesNPCManager : GlobalNPC
    {
        public static HashSet<int> EliteBlacklist { get; private set; }

        public override void Load()
        {
            EliteBlacklist = new HashSet<int>()
            {
                NPCID.LunarTowerNebula,
                NPCID.LunarTowerSolar,
                NPCID.LunarTowerStardust,
                NPCID.LunarTowerVortex,
                NPCID.MartianProbe,
                NPCID.BloodNautilus,
                NPCID.Mothron,
                NPCID.IceQueen,
                NPCID.Pumpking,
                NPCID.TheHungry,
                NPCID.TheHungryII,
                NPCID.PlanterasHook,
                NPCID.BigMimicCorruption,
                NPCID.BigMimicCrimson,
                NPCID.BigMimicHallow,
                NPCID.BigMimicJungle,
                NPCID.CultistArcherBlue,
                NPCID.CultistDevote,
                NPCID.CultistBossClone,
                NPCID.MartianDrone,
                NPCID.AngryNimbus,
                NPCID.DemonTaxCollector,
                NPCID.GiantTortoise,
                NPCID.RainbowSlime,
            };
        }

        public override void Unload()
        {
            EliteBlacklist?.Clear();
            EliteBlacklist = null;
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (Helpers.HereditarySource(source, out var parent) && parent is NPC parentNPC)
            {
                parentNPC.GetElitePrefixes(out var prefixes);
                foreach (var p in prefixes)
                {
                    npc.GetGlobalNPC(p).Active = true;
                }
            }
            else if (!npc.immortal && npc.damage > 0 && !NPCID.Sets.BelongsToInvasionOldOnesArmy[npc.type] && !npc.boss && !RORNPC.CountsAsBoss.Contains(npc.type) && !EliteBlacklist.Contains(npc.type) && !npc.IsElite())
            {
                var l = new List<EliteNPC>(RORNPC.RegisteredElites);
                while (l.Count > 0)
                {
                    int rolled = Main.rand.Next(l.Count);
                    if (!l[rolled].CanRoll(npc) || !Main.rand.NextBool(l[rolled].RollChance(npc)))
                    {
                        l.RemoveAt(rolled);
                        continue;
                    }
                    npc.GetGlobalNPC(l[rolled]).Active = true;
                }
            }
            npc.GetElitePrefixes(out var myPrefixes);
            foreach (var p in myPrefixes)
            {
                p.OnBecomeElite(npc);
            }
        }

        public override void ModifyTypeName(NPC npc, ref string typeName)
        {
            string prefixes = "";
            npc.GetElitePrefixes(out var list);
            foreach (var e in list)
            {
                if (!string.IsNullOrEmpty(prefixes))
                    prefixes += ", ";
                prefixes += e.Prefix;
            }
            typeName = prefixes + " " + typeName;
        }
    }
}