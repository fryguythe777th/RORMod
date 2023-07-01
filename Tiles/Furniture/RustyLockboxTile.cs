using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Items;
using RiskOfTerrain.Items.Consumable;
using RiskOfTerrain.Items.Placeable;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.Tiles.Furniture
{
    public class RustyLockboxTile : SecurityChestTile
    {
        public override bool RollSpawnChance(int i, int j, int tileType)
        {
            return WorldGen.genRand.NextBool(5);
        }

        public override void FillChest(int chestID, ref int index)
        {
            switch (WorldGen.genRand.NextFloat(1f))
            {
                case >= 0.8f:
                    {
                        var rolledItem = ChestDropInfo.RollChestItem(RORItem.RedTier, Main.chest[chestID].x, Main.chest[chestID].y, WorldGen.genRand);
                        if (rolledItem != null)
                        {
                            Main.chest[chestID].item[index++].SetDefaults(rolledItem.ItemID);
                        }
                    }
                    break;

                default:
                    {
                        var rolledItem = ChestDropInfo.RollChestItem(RORItem.GreenTier, Main.chest[chestID].x, Main.chest[chestID].y, WorldGen.genRand);
                        if (rolledItem != null)
                        {
                            Main.chest[chestID].item[index++].SetDefaults(rolledItem.ItemID);
                        }
                    }
                    break;
            }
        }

        public override void AddMapEntries()
        {
            AddMapEntry(Color.Brown, Language.GetText($"Mods.RiskOfTerrain.MapObject.{Name}"), MapChestName);

            AddMapEntry(Color.Brown, Language.GetText($"Mods.RiskOfTerrain.MapObject.{Name}Locked"), MapChestName);
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.tileOreFinderPriority[Type] = 550;
            Main.tileLighted[Type] = true;

            DustType = DustID.Iron;
        }

        public override bool CheckLocked(int i, int j, int left, int top, Player player)
        {
            return player.ConsumeItem(ModContent.ItemType<RustedKey>());
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<RustyLockbox>());
            Chest.DestroyChest(i, j);
        }

        public override int HoverItem(int i, int j, int left, int top)
        {
            if (Chest.IsLocked(left, top))
                return ModContent.ItemType<RustedKey>();
            return ModContent.ItemType<RustyLockbox>();
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0f;
            g = 0f;
            b = 0f;
            if (Main.tile[i, j].TileFrameX >= 36)
            {
                r = 0.33f;
                g = 0.15f;
                b = 0.01f;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
        }
    }
}