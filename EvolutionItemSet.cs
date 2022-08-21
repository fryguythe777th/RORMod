using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;

namespace ROR2Artifacts
{
    public struct EvolutionItemSet
    {
        public int ExpertAccessory;
        public List<Point> Accessories;

        public EvolutionItemSet(int expertAcc = ItemID.None)
        {
            ExpertAccessory = expertAcc;
            Accessories = new List<Point>();
        }
    }
}