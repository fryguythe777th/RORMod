using Microsoft.Xna.Framework;
using RiskOfTerrain.Items.Placeable;
using RiskOfTerrain.UI;
using RiskOfTerrain.UI.Terminal;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace RiskOfTerrain.Tiles
{
    public class TerminalTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16 };
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorInvalidTiles = new[] { (int)TileID.MagicalIceBlock, };
            TileObjectData.addTile(Type);
            DustType = DustID.Stone;
            AddMapEntry(new Color(128, 128, 128), CreateMapEntryName("Terminal"));
            MineResist = 3f;
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            var player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Terminal>();
        }


        public override bool RightClick(int i, int j)
        {
            RORUI.DynamicInterface.SetState(new TerminalUIState());
            return true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), (i + 1) * 16, (j + 3) * 16, 48, 48, ModContent.ItemType<Terminal>());
        }
    }
}