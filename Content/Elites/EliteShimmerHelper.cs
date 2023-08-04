using RiskOfTerrain;
using RiskOfTerrain.Content.Elites;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

public class EliteShimmerHelper : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public bool shimmering = false;
    public int shimmerTimer = 0;
    public List<EliteNPCBase> preShimmerPrefixes = new List<EliteNPCBase> { };

    public override void AI(NPC npc)
    {
        if (npc.shimmerWet && shimmering == false)
        {
            shimmering = true;
            shimmerTimer = 60;
        }

        if (shimmerTimer > 0)
        {
            shimmerTimer--;
        }

        if (shimmerTimer == 0 && shimmering == true)
        {
            if (preShimmerPrefixes.Count == 0)
            {
                npc.GetElitePrefixes(out var z);
                if (z.Count > 0)
                {
                    preShimmerPrefixes = z;
                    foreach (var p in z)
                    {
                        preShimmerPrefixes.Add(p);
                        npc.GetGlobalNPC(p).Active = false;
                    }
                    npc.GetGlobalNPC<TrollElite>().Active = true;
                }
            }
            else
            {
                npc.GetElitePrefixes(out var z);
                preShimmerPrefixes = z;
                foreach (var p in z)
                {
                    npc.GetGlobalNPC(p).Active = false;
                }

                foreach (var p in preShimmerPrefixes)
                {
                    npc.GetGlobalNPC(p).Active = true;
                }

                preShimmerPrefixes.Clear();
            }
        }

        if (!npc.shimmerWet)
        {
            shimmering = false;
        }
    }
}