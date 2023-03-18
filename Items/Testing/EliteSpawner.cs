using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Elites;
using Terraria.ID;
using RiskOfTerrain.NPCs;
using System.Collections.Generic;

namespace RiskOfTerrain.Items.Testing
{
    public class EliteSpawner : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.RegenerationPotion);
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            player.statLifeMax = 400;
            int i = NPC.NewNPC(NPC.GetSource_None(), (int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, NPCID.SkeletonArcher);

            NPC npc = Main.npc[i];
            var l = new List<EliteNPCBase>(RORNPC.RegisteredElites);
            npc.GetGlobalNPC(l[1]).Active = true;
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

            return true;
        }
    }
}