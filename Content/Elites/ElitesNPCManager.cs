using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Elites
{
    public class EliteNPCManager : GlobalNPC
    {
        public static HashSet<int> EliteBlacklist { get; private set; }

        public static bool DrawingElite;

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
                NPCID.SpikeBall,
                NPCID.CultistTablet,
                NPCID.BlackRecluse,
                NPCID.BlackRecluseWall,
                NPCID.JungleCreeper,
                NPCID.JungleCreeperWall,
                NPCID.BloodCrawler,
                NPCID.BloodCrawlerWall,
                NPCID.DesertScorpionWalk,
                NPCID.DesertScorpionWall,

                ModContent.NPCType<MalachiteUrchin>(),
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
                    if (p.Name != "CelestineElite" && p.Name != "MendingElite")
                    {
                        npc.GetGlobalNPC(p).Active = true;
                    }
                }
            }
            else if (Main.netMode != NetmodeID.MultiplayerClient && !npc.townNPC && !npc.friendly && !NPCID.Sets.CountsAsCritter[npc.type] && !npc.immortal && npc.damage > 0 && !NPCID.Sets.BelongsToInvasionOldOnesArmy[npc.type] && !npc.boss && !RORNPC.CountsAsBoss.Contains(npc.type) && !EliteBlacklist.Contains(npc.type) && !npc.IsElite())
            {
                var l = new List<EliteNPCBase>(RORNPC.RegisteredElites);
                while (l.Count > 0)
                {
                    int rolled = Main.rand.Next(l.Count);
                    if (!l[rolled].CanRoll(npc) || !Main.rand.NextBool(l[rolled].RollChance(npc)))
                    {
                        l.RemoveAt(rolled);
                        continue;
                    }
                    npc.GetGlobalNPC(l[rolled]).Active = true;
                    l.RemoveAt(rolled);
                }
            }
            npc.GetElitePrefixes(out var myPrefixes);
            if (myPrefixes.Count > 0)
            {
                npc.netUpdate = true;
                npc.ROR().syncLifeMax = true;
            }
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

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            npc.GetElitePrefixes(out var list);
            foreach (var e in list)
            {
                if (e.Shader != null)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.Transform);
                    e.Shader.Apply(npc);
                    DrawingElite = true;
                    return true;
                }
            }
            if (DrawingElite)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.Transform);
                DrawingElite = false;
            }
            return true;
        }
    }
}