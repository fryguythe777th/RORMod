using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using ReLogic.Content;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Miner
{
    public class MinerPickaxeWeapon : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 38;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Orange;

            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 17;
            Item.knockBack = 5;
            Item.pick = 165;
        }

        public int dashCooldown = 0; //set to 480
        public int dashTime = 0;
        public float savedY = 0;

        public override void HoldItem(Player player)
        {
            RORPlayer miner = player.ROR();

            if (miner.minerDashCharge > 0)
            {
                if (miner.minerFuel < 1000)
                {
                    miner.minerMaxCharge = 120;
                }

                if (miner.minerFuel >= 1000 && miner.minerFuel < 1500)
                {
                    miner.minerMaxCharge = 90;
                }

                if (miner.minerFuel >= 1500 && miner.minerFuel <= 2000)
                {
                    miner.minerMaxCharge = 60;
                }
            }

            if (Main.mouseRight && dashCooldown == 0)
            {
                miner.minerDashCharge++;
            }

            if (!Main.mouseRight)
            {
                miner.minerDashCharge = 0;
            }

            if (miner.minerDashCharge == miner.minerMaxCharge)
            {
                miner.minerDashCharge = 0;
                dashCooldown = 480;
                dashTime = 30;
                player.eocDash = 30;
                savedY = player.position.Y;
            }
            
            if (dashCooldown > 0)
            {
                dashCooldown--;
            }

            if (dashTime > 0)
            {
                player.velocity = new Vector2(player.direction * 10, 0);
                player.position.Y = savedY;
            }

            if (dashTime == 0)
            {
                player.eocDash = 0;
            }
        }

        public override void UpdateInventory(Player player)
        {
            if (dashTime > 0)
            {
                dashTime--;

                player.controlJump = false;
                player.controlDown = false;
                player.controlLeft = false;
                player.controlRight = false;
                player.controlUp = false;
                player.controlUseItem = false;
                player.controlUseTile = false;
                player.controlThrow = false;

                for (int playerTileCoordsX = (int)(player.position.X / 16f) - 2; playerTileCoordsX <= ((player.position.X + player.width) / 16f) + 4; playerTileCoordsX++)
                {
                    for (int playerTileCoordsY = (int)(player.position.Y / 16f) + (1 / 2); playerTileCoordsY <= ((player.position.Y + player.height) / 16f) - (1); playerTileCoordsY++)
                    {
                        if (WorldGen.SolidTile3(playerTileCoordsX, playerTileCoordsY))
                        {
                            if (WorldGen.TileType(playerTileCoordsX, playerTileCoordsY) == TileID.Chlorophyte || WorldGen.TileType(playerTileCoordsX, playerTileCoordsY) == TileID.LihzahrdBrick)
                            {
                                dashTime = 0;
                            }
                            else
                            {
                                WorldGen.KillTile(playerTileCoordsX, playerTileCoordsY);

                                if (player.ROR().minerSetBonusActive)
                                {
                                    RORPlayer miner = player.ROR();
                                    miner.minerFuel += 20;
                                    if (miner.minerFuel > 2000)
                                    {
                                        miner.minerFuel = 2000;
                                    }
                                }
                            }
                        }
                    }
                }

                player.velocity = new Vector2(player.direction * 10, 0);
                player.position.Y = savedY;

                player.immune = true;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (!Main.npc[i].friendly && Main.npc[i].lifeMax > 5 && Main.npc[i].damage > 0 && Main.npc[i].active && Main.npc[i].Hitbox.Intersects(player.Hitbox))
                    {
                        NPC.HitInfo hit = new NPC.HitInfo
                        {
                            DamageType = DamageClass.Default,
                            SourceDamage = 40,
                            Damage = 40,
                            Crit = false,
                            Knockback = 10f,
                            HitDirection = player.direction
                        };
                        Main.npc[i].StrikeNPC(hit);
                        NetMessage.SendStrikeNPC(Main.npc[i], hit);
                    }
                }

                //axis of awesome 4 chords
                //mix and match lyrics
                //stairway to gilligan's island
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (dashTime > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}