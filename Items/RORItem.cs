using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace RORMod.Items
{
    public class RORItem : GlobalItem
    {
        public static List<int> WhiteTier { get; private set; }
        public static List<int> GreenTier { get; private set; }
        public static List<int> RedTier { get; private set; }
        public static List<int> BossTier { get; private set; }
        public static List<int> LunarTier { get; private set; }
        public static List<int> VoidTier { get; private set; }

        public override void Load()
        {
            WhiteTier = new List<int>();
            GreenTier = new List<int>();
            RedTier = new List<int>();
            BossTier = new List<int>();
            LunarTier = new List<int>();
            VoidTier = new List<int>();
        }
    }
}