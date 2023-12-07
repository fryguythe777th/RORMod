using Microsoft.Xna.Framework;
using RiskOfTerrain.Tiles.Furniture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Tiles
{
    public class RORTile : GlobalTile
    {
        public static List<SecurityChestTile> SecurityChests { get; private set; }
        public static HashSet<int> ChestsSpawnPreventionIDs { get; private set; }

        public override void Load()
        {
            SecurityChests = new List<SecurityChestTile>();
            ChestsSpawnPreventionIDs = new HashSet<int>();
        }

        public override void Unload()
        {
            ChestsSpawnPreventionIDs?.Clear();
            ChestsSpawnPreventionIDs = null;
            SecurityChests?.Clear();
            SecurityChests = null;
        }

        public static SecurityChestTile RollSecurityChestToSpawn(int i, int j, int tileType)
        {
            for (int k = 0; k < 100; k++)
            {
                var s = WorldGen.genRand.Next(SecurityChests);
                if (s.RollSpawnChance(i, j, tileType))
                    return s;
            }
            return null;
        }
        public static bool IsTileInView(int x, int y, int rectSize = 10)
        {
            var spawnRectangle = new Rectangle(x * 16 - rectSize * 16, y * 16 - rectSize * 16, rectSize * 32, rectSize * 32);
            for (int k = 0; k < Main.maxPlayers; k++)
            {
                if (Main.player[k].active && Main.player[k].GetViewRectangle().Intersects(spawnRectangle))
                {
                    return true;
                }
            }
            return false;
        }
        public override void RandomUpdate(int i, int j, int type)
        {
            if (j > Main.UnderworldLayer)
            {
                return;
            }
            if (WorldGen.genRand.NextBool((int)(10000 / ServerConfig.Instance.ChestSpawnFrequency)) && Main.tile[i, j].HasUnactuatedTile && Main.tileSolid[Main.tile[i, j].TileType] && !Main.tileSolidTop[Main.tile[i, j].TileType])
            {
                var s = RollSecurityChestToSpawn(i, j, type);
                if (s == null || IsTileInView(i, j, 20))
                    return;

                int size = 80;
                int endX = Math.Min(i + size, Main.maxTilesX - size);
                int endY = Math.Min(j + size, Main.maxTilesY - size);

                for (int k = Math.Clamp(i - size, size, Main.maxTilesX - size); k < endX; k++)
                {
                    for (int l = Math.Clamp(j - size, size, Main.maxTilesY - size); l < endY; l++)
                    {
                        if (Main.tile[k, l].HasTile && ChestsSpawnPreventionIDs.Contains(Main.tile[k, l].TileType))
                        {
                            return;
                        }
                    }
                }

                endX = Math.Min(i + 2, Main.maxTilesX - 2);
                endY = Math.Min(j + 2, Main.maxTilesY - 2);

                for (int k = Math.Clamp(i - 2, 50, Main.maxTilesX - 2); k < endX; k++)
                {
                    for (int l = Math.Clamp(j - 2, 50, Main.maxTilesY - 2); l < endY; l++)
                    {
                        int chestID = WorldGen.PlaceChest(k, l, s.Type, notNearOtherChests: false, style: 1);
                        if (chestID != -1)
                        {
                            int index = 0;
                            s.FillChest(chestID, ref index);
                            //Main.LocalPlayer.Teleport(new Vector2(k * 16f, l * 16f));
                            return;
                        }
                    }
                }
            }
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Player closest = Main.player[Player.FindClosest(new Vector2(i, j), 1, 1)];

            if (closest.ROR().minerSetBonusActive)
            {
                RORPlayer miner = closest.ROR();
                miner.minerFuel += 20;
                if (miner.minerFuel > 2000)
                {
                    miner.minerFuel = 2000;
                }
            }
        }
    }
}