using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Items;
using RiskOfTerrain.Items.Consumable;
using RiskOfTerrain.Items.Placeable;
using RiskOfTerrain.UI;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace RiskOfTerrain.Tiles.Furniture
{
    /// <summary>
    /// NOTE: These are forced to be 2x2 tiles and use frames of 18x18! If you decide to add a tile which inherits from this, and changes these dimensions, please check the logic in this class, and the logic in <see cref="RORItem.UseCheckLock(Item, Player)"/>
    /// </summary>
    public class SecurityChestTile : ModTile
    {
        public virtual Color MapColor => new Color(20, 80, 200);

        public virtual bool RollSpawnChance(int i, int j, int tileType)
        {
            return true;
        }

        public virtual void FillChest(int chestID, ref int index)
        {
            switch (WorldGen.genRand.NextFloat(1f))
            {
                case <= 0.19f:
                    {
                        var rolledItem = ChestDropInfo.RollChestItem(RORItem.GreenTier, Main.chest[chestID].x, Main.chest[chestID].y, WorldGen.genRand);
                        if (rolledItem != null)
                        {
                            Main.chest[chestID].item[index++].SetDefaults(rolledItem.ItemID);
                        }
                    }
                    break;

                case <= 0.20f:
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
                        var rolledItem = ChestDropInfo.RollChestItem(RORItem.WhiteTier, Main.chest[chestID].x, Main.chest[chestID].y, WorldGen.genRand);
                        if (rolledItem != null)
                        {
                            Main.chest[chestID].item[index++].SetDefaults(rolledItem.ItemID);
                        }
                    }
                    break;
            }
            if (WorldGen.genRand.NextBool(10))
            {
                Main.chest[chestID].item[index++].SetDefaults(ModContent.ItemType<RustedKey>());
            }
            if (WorldGen.genRand.NextBool())
            {
                Main.chest[chestID].item[index].SetDefaults(ModContent.ItemType<PowerElixir>());
                Main.chest[chestID].item[index++].stack = WorldGen.genRand.Next(2) + 1;
            }
            if (WorldGen.genRand.NextBool())
            {
                Main.chest[chestID].item[index].SetDefaults(ModContent.ItemType<BisonSteak>());
                Main.chest[chestID].item[index++].stack = WorldGen.genRand.Next(2) + 1;
            }
        }

        public virtual void AddMapEntries()
        {
            AddMapEntry(MapColor, this.GetLocalization("MapEntry", PrettyPrintName), MapChestName);
            AddMapEntry(MapColor, this.GetLocalization("MapEntry_Locked", PrettyPrintName), MapChestName);
        }

        public override void SetStaticDefaults()
        {
            Main.tileSpelunker[Type] = true;
            Main.tileContainer[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileOreFinderPriority[Type] = 500;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.BasicChest[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            DustType = DustID.Cobalt;
            RegisterItemDrop(ModContent.ItemType<SecurityChest>());
            AdjTiles = new int[] { TileID.Containers };

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
            TileObjectData.newTile.AnchorInvalidTiles = new int[] { TileID.MagicalIceBlock };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);

            AddMapEntries();

            RORTile.SecurityChests.Add(this);
            RORTile.ChestsSpawnPreventionIDs.Add(Type);
        }

        public override ushort GetMapOption(int i, int j)
        {
            return (ushort)(Main.tile[i, j].TileFrameX / 36);
        }

        public override LocalizedText DefaultContainerName(int frameX, int frameY) {
            return frameX >= 36 ? this.GetLocalization("MapEntry_Locked", PrettyPrintName) : this.GetLocalization("MapEntry", PrettyPrintName);
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override bool IsLockedChest(int i, int j)
        {
            return Main.tile[i, j].TileFrameX / 36 == 1;
        }

        public override bool UnlockChest(int i, int j, ref short frameXAdjustment, ref int dustType, ref bool manual)
        {
            dustType = DustType;
            return true;
        }

        public static string MapChestName(string name, int i, int j)
        {
            int left = i;
            int top = j;
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX % 36 != 0)
            {
                left--;
            }

            if (tile.TileFrameY != 0)
            {
                top--;
            }

            int chest = Chest.FindChest(left, top);
            if (chest < 0)
            {
                return Language.GetTextValue("LegacyChestType.0");
            }

            if (Main.chest[chest].name == "")
            {
                return name;
            }

            return name + ": " + Main.chest[chest].name;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Chest.DestroyChest(i, j);
        }

        public virtual int CalculateChestPrice()
        {
            return RiskOfTerrain.CalculateChestPrice(1);
        }

        public virtual bool CheckLocked(int i, int j, int left, int top, Player player)
        {
            return player.CanAfford(CalculateChestPrice(), -1);
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            Main.mouseRightRelease = false;
            int left = i;
            int top = j;
            if (tile.TileFrameX % 36 != 0)
            {
                left--;
            }

            if (tile.TileFrameY != 0)
            {
                top--;
            }

            player.CloseSign();
            player.SetTalkNPC(-1);
            Main.npcChatCornerItem = 0;
            Main.npcChatText = "";
            if (Main.editChest)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Main.editChest = false;
                Main.npcChatText = string.Empty;
            }

            if (player.editedChestName)
            {
                NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
                player.editedChestName = false;
            }

            bool isLocked = Chest.IsLocked(left, top);
            if (Main.netMode == NetmodeID.MultiplayerClient && !isLocked)
            {
                if (left == player.chestX && top == player.chestY && player.chest >= 0)
                {
                    player.chest = -1;
                    Recipe.FindRecipes();
                    SoundEngine.PlaySound(SoundID.MenuClose);
                }
                else
                {
                    NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top);
                    Main.stackSplit = 600;
                }
            }
            else
            {
                if (isLocked)
                {
                    if (CheckLocked(i, j, left, top, player) && Chest.Unlock(left, top))
                    {
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendData(MessageID.LockAndUnlock, -1, -1, null, player.whoAmI, 1f, left, top);
                        }
                    }
                }
                else
                {
                    int chest = Chest.FindChest(left, top);
                    if (chest >= 0)
                    {
                        Main.stackSplit = 600;
                        if (chest == player.chest)
                        {
                            player.chest = -1;
                            SoundEngine.PlaySound(SoundID.MenuClose);
                        }
                        else
                        {
                            SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
                            player.OpenChest(left, top, chest);
                        }

                        Recipe.FindRecipes();
                    }
                }
            }

            return true;
        }

        public virtual int HoverItem(int i, int j, int left, int top)
        {
            if (Chest.IsLocked(left, top))
                return ItemID.GoldCoin;
            return ModContent.ItemType<SecurityChest>();
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            if (tile.TileFrameX % 36 != 0)
            {
                left--;
            }

            if (tile.TileFrameY != 0)
            {
                top--;
            }

            int chest = Chest.FindChest(left, top);
            player.cursorItemIconID = -1;
            if (chest < 0)
            {
                player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
            }
            else
            {
                string defaultName = TileLoader.DefaultContainerName(tile.TileType, i, j)/* tModPorter Note: new method takes in FrameX and FrameY */;
                player.cursorItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : defaultName;
                if (player.cursorItemIconText == defaultName)
                {
                    player.cursorItemIconID = HoverItem(i, j, left, top);
                    player.cursorItemIconText = "";
                }
            }

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }

        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
            Player player = Main.LocalPlayer;
            if (player.cursorItemIconText == "")
            {
                player.cursorItemIconEnabled = false;
                player.cursorItemIconID = 0;
            }
        }

        public override void RandomUpdate(int i, int j)
        {
            if (Main.tile[i, j].TileFrameX == 36 && Main.tile[i, j].TileFrameY == 0 && WorldGen.genRand.NextBool(100) && !RORTile.IsTileInView(i, j, 10))
            {
                for (int x = i; x <= i + 1; x++)
                {
                    for (int y = j; y <= j + 1; y++)
                    {
                        var t = Main.tile[x, y];
                        t.HasTile = false;
                    }
                }
                NetMessage.SendTileSquare(-1, i, j, 2, 2);
                Chest.DestroyChest(i, j);
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            try
            {
                var tile = Main.tile[i, j];
                int left = i;
                int top = j;
                if (tile.TileFrameX % 36 != 0)
                {
                    left--;
                }
                if (tile.TileFrameY != 0)
                {
                    top--;
                }
                int chest = Chest.FindChest(left, top);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>($"{Texture}_Glow", AssetRequestMode.ImmediateLoad).Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + Helpers.TileDrawOffset,
                    new Rectangle(tile.TileFrameX, 38 * (chest == -1 ? 0 : Main.chest[chest].frame) + tile.TileFrameY, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                if (Chest.IsLocked(left, top) && !PurchasableChestInterface.PurchasePopups.ContainsKey(new Point(left, top)))
                {
                    SpawnPopupText(left, top);
                }
            }
            catch
            {
            }
        }

        public virtual void SpawnPopupText(int left, int top)
        {
            if (Vector2.Distance(Main.LocalPlayer.Center, new Vector2(left * 16f + 16f, top * 16f + 16f)) < 100f)
            {
                PurchasableChestInterface.PurchasePopups.Add(new Point(left, top), new PurchasableChestInterface.ChestPurchasePopup(left, top, CalculateChestPrice(), 1, 1));
            }
        }
    }
}