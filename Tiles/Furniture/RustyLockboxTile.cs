using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Items;
using RiskOfTerrain.Items.Consumable;
using RiskOfTerrain.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Tiles.Furniture
{
    public class RustyLockboxTile : SecurityChestTile
    {
        public override bool RollSpawnChance(int i, int j, int tileType)
        {
            return WorldGen.genRand.NextBool(8);
        }

        public override void FillChest(int chestID, ref int index)
        {
            switch (WorldGen.genRand.NextFloat(1f))
            {
                case >= 0.8f:
                    Main.chest[chestID].item[index++].SetDefaults(Main.rand.Next(RORItem.RedTier));
                    break;

                default:
                    Main.chest[chestID].item[index++].SetDefaults(Main.rand.Next(RORItem.GreenTier));
                    break;
            }
        }

        public override void AddMapEntries()
        {
            var name = CreateMapEntryName();
            AddMapEntry(Color.Brown, name, MapChestName);

            name = CreateMapEntryName(Name + "Locked");
            AddMapEntry(Color.Brown, name, MapChestName);
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.tileOreFinderPriority[Type] = 550;
            Main.tileLighted[Type] = true;

            DustType = DustID.Iron;
            ChestDrop = ModContent.ItemType<RustyLockbox>();
        }

        public override bool CheckLocked(int i, int j, int left, int top, Player player)
        {
            return player.ConsumeItem(ModContent.ItemType<RustedKey>());
        }

        public override int HoverItem(int i, int j, int left, int top)
        {
            if (Chest.IsLocked(left, top))
                return ModContent.ItemType<RustedKey>();
            return ChestDrop;
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