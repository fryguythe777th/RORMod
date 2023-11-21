using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.NPCs.Enemies.Gup
{
    public class Gup : ModNPC
    {
        private static Asset<Texture2D> gupSpikesTexture;

        private enum AIStates
        {
            Idle,
            Seeking,
            Spiking
        }

        private enum Frames //17 total, 158 (79) tall each
        {
            Regular, //
            Squish1, //
            Squish2, //
            Squish3, //
            SpikePrepare, //
            BoingPrepare, //
            BoingUp, //
            BoingDown, //
            Spike1, //
            Spike2 //
        }

        public ref float AIState => ref NPC.ai[0];
        public ref float Size => ref NPC.ai[2]; //size0 big, size1 medium, size2 baby
        public float sizeScaleFactor;
        public int frame;
        public int aiTimer;

        public int jumpCounter = 0;
        public bool reversion = false;

        public override void Load()
        {
            gupSpikesTexture = ModContent.Request<Texture2D>("RiskOfTerrain/NPCs/Enemies/Gup/Gup_Spikes");
        }

        public override void Unload()
        {
            gupSpikesTexture = null;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                new FlavorTextBestiaryInfoElement("Mods.RiskOfTerrain.Bestiary.Gup"),
            });
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers();
            drawModifiers.PortraitPositionYOverride = 2;
            drawModifiers.Scale = 1.1f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = drawModifiers;
        }

        public override void SetDefaults()
        {
            NPC.width = 180;
            NPC.height = 174;
            NPC.aiStyle = -1;
            NPC.damage = 50;
            NPC.lifeMax = 4332;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 1400;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneJungle && !spawnInfo.PlayerSafe && Main.netMode != NetmodeID.Server ? 0.1f : 0f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (NPC.ai[1] == 0)
            {
                bool sizePicked = false;
                while (!sizePicked)
                {
                    int choice = Main.rand.Next(0, 3);

                    switch (choice)
                    {
                        case 0:
                            {
                                if (Main.hardMode)
                                {
                                    sizePicked = true;
                                    Size = choice;
                                }
                                break;
                            }
                        case 1:
                            {
                                if (NPC.downedBoss2)
                                {
                                    sizePicked = true;
                                    Size = choice;
                                }
                                break;
                            }
                        case 2:
                            {
                                sizePicked = true;
                                Size = choice;
                                break;
                            }
                    }
                }
            }

            AIState = (int)AIStates.Idle;
            sizeScaleFactor = 1 + (Size / 2);
            NPC.width = (int)(180 / sizeScaleFactor);
            NPC.height = (int)(180 / sizeScaleFactor);
            NPC.lifeMax = (int)(1000 / sizeScaleFactor);
            NPC.life = NPC.lifeMax;
            NPC.scale = 1 / sizeScaleFactor;
            NPC.alpha = (int)(10 * Size);
            NPC.damage = 0;
            NPC.value = 200;
            NPC.GivenName = Language.GetTextValue("Mods.RiskOfTerrain.NPCs.Gup.NameSize" + Size);

            if (NPC.ai[1] == 0)
            {
                while (Collision.SolidCollision(NPC.position, NPC.width, NPC.height + 1, true))
                {
                    NPC.position.Y--;
                }
            }
        }

        public override void AI()
        {
            if (!Collision.SolidCollision(NPC.position, NPC.width, NPC.height + 1, true))
            {
                NPC.velocity.Y += 0.5f;
            }

            if (NPC.velocity.Y == 0)
            {
                NPC.velocity.X /= 4 - Size;
            }

            if (Math.Sign(NPC.velocity.Y) == -1)
            {
                if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height + 1, false))
                    NPC.velocity.Y = 0;
            }
            else if (Math.Sign(NPC.velocity.Y) == 1)
            {
                if (Collision.SolidCollision(new Vector2(NPC.position.X, NPC.position.Y + NPC.height), NPC.width, 1, true))
                    NPC.velocity.Y = 0;
            }

            switch (AIState)
            {
                case (float)AIStates.Idle:
                    WaitForPlayer();
                    break;
                case (float)AIStates.Seeking:
                    ChasePlayer();
                    break;
                case (float)AIStates.Spiking:
                    SpikeUp();
                    break;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;

            NPC.frame.Y = frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (NPC.direction == 1)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            if (frame == (int)Frames.Spike1 || frame == (int)Frames.Spike2)
            {
                Color sillyColor = new Color(drawColor.R, drawColor.G, drawColor.B, 255 - (10 * Size));
                Main.EntitySpriteDraw(gupSpikesTexture.Value, NPC.Center - Main.screenPosition + new Vector2(0, 2), new Rectangle(gupSpikesTexture.Value.Bounds.Left, (int)(gupSpikesTexture.Value.Bounds.Size().Y / 2) * (frame - 8), gupSpikesTexture.Value.Bounds.Width, gupSpikesTexture.Value.Bounds.Height / 2), sillyColor, NPC.rotation, new Vector2(gupSpikesTexture.Size().X * 0.5f, gupSpikesTexture.Size().Y * 0.25f), 1 / sizeScaleFactor, effect, 0);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void WaitForPlayer()
        {
            int closest = NPC.FindClosestPlayer(out float distanceToPlayer);
            frame = (int)Frames.Regular;

            if (distanceToPlayer < 800)
            {
                AIState = (int)AIStates.Seeking;
            }
        }

        private void ChasePlayer()
        {
            int closest = NPC.FindClosestPlayer(out float distanceToPlayer);
            int distanceV = (int)(Main.player[closest].position.X - NPC.position.X);

            if (NPC.velocity.Y == 0)
            {
                int correctedDistanceV = distanceV == 0 ? 1 : distanceV;
                NPC.direction = distanceV / Math.Abs(correctedDistanceV);

                if (distanceToPlayer < 150)
                {
                    jumpCounter = 0;
                    AIState = (int)AIStates.Spiking;
                    aiTimer = 120;
                    frame = (int)Frames.SpikePrepare;
                }

                if (distanceToPlayer >= 150 && distanceToPlayer <= 800)
                {
                    if (aiTimer > 0)
                    {
                        aiTimer--;
                    }
                    else
                    {
                        if (frame == (int)Frames.BoingUp)
                        {
                            frame = (int)Frames.Regular;
                        }
                        if (frame == (int)Frames.Regular)
                        {
                            aiTimer = 6;
                            frame = (int)Frames.Squish1;
                            reversion = false;
                        }
                        else if (frame == (int)Frames.Squish1 && !reversion)
                        {
                            aiTimer = 6;
                            frame = (int)Frames.Squish2;
                            reversion = false;
                        }
                        else if (frame == (int)Frames.Squish2 && !reversion)
                        {
                            aiTimer = 6;
                            frame = (int)Frames.Squish3;
                            reversion = false;
                        }
                        else if (frame == (int)Frames.Squish3 && !reversion)
                        {
                            aiTimer = 6;
                            frame = (int)Frames.Squish2;
                            reversion = true;
                        }
                        else if (frame == (int)Frames.Squish2 && reversion)
                        {
                            aiTimer = 6;
                            frame = (int)Frames.Squish1;
                            reversion = true;
                        }
                        else if (frame == (int)Frames.Squish1 && reversion)
                        {
                            aiTimer = 2;
                            frame = (int)Frames.Regular;
                            reversion = false;

                            if (jumpCounter == 0 | jumpCounter == 1)
                            {
                                NPC.velocity = new Vector2(4 * NPC.direction, -10 / sizeScaleFactor);
                                jumpCounter++;
                            }
                            else
                            {
                                jumpCounter = 0;
                                NPC.velocity = new Vector2(4 * NPC.direction, -15 / sizeScaleFactor);
                                frame = (int)Frames.BoingUp;
                            }
                        }
                    }
                }

                if (distanceToPlayer > 800)
                {
                    AIState = (int)AIStates.Idle;
                }
            }

            if (frame == (int)Frames.BoingUp)
            {
                NPC.velocity.X = 4 * NPC.direction;
            }
        }

        private void SpikeUp()
        {
            if (aiTimer > 0)
            {
                aiTimer--;
            }
            else
            {
                if (frame == (int)Frames.SpikePrepare)
                {
                    aiTimer = 10;
                    frame = (int)Frames.Spike1;
                    NPC.width = (int)(260 / sizeScaleFactor);
                    NPC.position.X -= (int)(40 / sizeScaleFactor);
                }
                else if (frame == (int)Frames.Spike1)
                {
                    aiTimer = 180;
                    frame = (int)Frames.Spike2;
                    NPC.damage = (int)(50 / sizeScaleFactor);
                }
                else if (frame == (int)Frames.Spike2)
                {
                    NPC.width = (int)(180 / sizeScaleFactor);
                    NPC.position.X += (int)(40 / sizeScaleFactor);
                    NPC.damage = 0;
                    frame = (int)Frames.Regular;
                    AIState = (int)AIStates.Seeking;
                }
            }
        }

        public override void OnKill()
        { 
            if (Size < 2)
            {
                for (int i = -1; i < 2; i += 2)
                {
                    int j = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Gup>(), ai0: 0, ai1: 1, ai2: Size + 1);
                    Main.npc[j].velocity = new Vector2(i * 5, 0);
                    Main.npc[j].ai[1] = 1;
                }
            }

            SoundEngine.PlaySound(SoundID.NPCDeath1);

            for (int i = 0; i < 40 / sizeScaleFactor; i++)
            {
                int scale = Main.rand.Next(1, 3);
                Dust.NewDust(NPC.Center, scale, scale, ModContent.DustType<GupDust>(), Main.rand.Next(-2, 3), Main.rand.Next(-3, 2));
            }

            int k = Item.NewItem(NPC.GetSource_DropAsItem(), new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), ItemID.Gel, Main.rand.Next(1, 6));
            Main.item[k].color = Color.Orange;
        }
    }

    public class GupDust : ModDust
    {
        public float initialVelX = 0;
        public float initialPosX = 0;

        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 25;
            dust.noLight = true;
            dust.noGravity = false;
            initialVelX = dust.velocity.X;
            initialPosX = dust.position.X;
        }
    }
}