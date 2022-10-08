using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;

namespace RiskOfTerrain.Content.Artifacts
{
    public struct NPCEquips
    {
        public int ExpertAccessory;
        public List<Point> Accessories;

        public NPCEquips(int expertAcc = ItemID.None)
        {
            ExpertAccessory = expertAcc;
            Accessories = new List<Point>();
        }
    }
}