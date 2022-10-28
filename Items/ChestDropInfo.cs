using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Utilities;

namespace RiskOfTerrain.Items
{
    public class ChestDropInfo
    {
        public delegate bool CanRollItemFromChest(int i, int j);

        public int ItemID;
        public CanRollItemFromChest FromChest;
        public Func<bool> CanRoll;

        public static ChestDropInfo RollChestItem(List<ChestDropInfo> dropInfo, int i, int j, UnifiedRandom rand)
        {
            for (int k = 0; k < 100; k++)
            {
                var drop = rand.Next(dropInfo);
                if (drop?.FromChest?.Invoke(i, j) == false || drop?.CanRoll?.Invoke() == false)
                {
                    continue;
                }
                return drop;
            }
            return null;
        }

        public static implicit operator ChestDropInfo(int type)
        {
            return new ChestDropInfo() { ItemID = type, };
        }

        public static implicit operator ChestDropInfo((int, Func<bool>) value)
        {
            return new ChestDropInfo() { ItemID = value.Item1, CanRoll = value.Item2 };
        }
    }
}
