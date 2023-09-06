using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Content;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Content.Elites;
using RiskOfTerrain.Items.Accessories.T1Common;
using RiskOfTerrain.Items.CharacterSets.Artificer;
using RiskOfTerrain.Items.Consumable;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace RiskOfTerrain.NPCs
{
    public class RORNPC : GlobalNPC
    {
        public byte bleedingStacks;
        public bool bleedShatterspleen;
        public bool syncLifeMax;
        public bool convertToCursedFlames;

        public int lastHitDamage;
        public int gasolineDamage;

        public float statDefense;

        private bool drawConfused;

        public bool chronobaubleToggle;

        public float npcSpeedStat;
        public int npcSpeedUpdate;
        public int npcSpeedRepeatCounter = 0;

        public bool hasBeenStruckByUkuleleLightning;

        public float shield;
        public float maxShield;
        public int timeSinceLastHit = 300;
        public int regularMaxLife;
        public int savedLife;
        public bool isAShielder = false;

        public int shatterizationCount = 0;
        public int scorpionCount = 0;

        public int savedAlpha = -1;

        public int lastHitProjectileType = -1;

        public bool isMending;

        public bool mostRecentHitHadMask = false;
        public int happiestMaskHolder = -1;

        public bool warHornHasBeenHit { get; set; }

        public override bool InstancePerEntity => true;

        public static List<EliteNPCBase> RegisteredElites { get; private set; }

        public static List<Vector2> LightningDrawPoints { get; private set; }

        /// <summary>
        /// Used for HP bars and Armor Piercing Rounds for NPCs not flagged as a boss but should be treated as such
        /// </summary>
        public static HashSet<int> CountsAsBoss { get; private set; }

        public override void Load()
        {
            CountsAsBoss = new HashSet<int>()
            {
                NPCID.GolemFistLeft,
                NPCID.GolemFistRight,
                NPCID.GolemHead,
                NPCID.SkeletronHand,
                NPCID.PrimeCannon,
                NPCID.PrimeLaser,
                NPCID.PrimeSaw,
                NPCID.PrimeVice,
                NPCID.TheDestroyerBody,
                NPCID.TheDestroyerTail,
                NPCID.BloodNautilus,
                NPCID.PirateShipCannon,
                NPCID.MourningWood,
                NPCID.Pumpking,
                NPCID.Everscream,
                NPCID.SantaNK1,
                NPCID.IceQueen,
                NPCID.MartianSaucerCannon,
                NPCID.MartianSaucerTurret,
                NPCID.DD2Betsy,
                NPCID.DD2DarkMageT1,
                NPCID.DD2DarkMageT3,
                NPCID.DD2OgreT2,
                NPCID.DD2OgreT3,
            };

            RegisteredElites = new List<EliteNPCBase>();
            LightningDrawPoints = new List<Vector2>();
            Terraria.On_NPC.StrikeNPC_int_float_int_bool_bool_bool += NPC_StrikeNPC;
            Terraria.On_NPC.UpdateNPC_Inner += NPC_UpdateNPC;

            shatterizationCount = 0;
        }

        private int NPC_StrikeNPC(On_NPC.orig_StrikeNPC_int_float_int_bool_bool_bool orig, NPC self, int Damage, float knockBack, int hitDirection, bool crit, bool fromNet, bool noPlayerInteraction)
        {
            //if (self.TryGetGlobalNPC<RORNPC>(out var ror))
            //{
            //    ror.lastHitDamage = Damage;
            //}
            return orig(self, Damage, knockBack, hitDirection, crit, fromNet, noPlayerInteraction);
        }

        private void NPC_UpdateNPC(Terraria.On_NPC.orig_UpdateNPC_Inner orig, NPC self, int i)
        {
            orig(self, i);

            float extraLoops = self.TryGetGlobalNPC(out RORNPC E) ? E.npcSpeedStat : 0;

            if (extraLoops > 1f)
            {
                for (int j = 0; j < (int)Math.Round(extraLoops) - 1; j++)
                {
                    orig(self, i);
                }
            }
        }

        public static int Distance(Entity entityA, Entity entityB)
        {
            //the distance formula
            //may god smite the inventor of honors geometry
            int xa = (int)entityA.Center.X;
            int ya = (int)entityA.Center.Y;
            int xb = (int)entityB.Center.X;
            int yb = (int)entityB.Center.Y;
            return (int)Math.Round(Math.Sqrt((xb - xa) * (xb - xa) + (yb - ya) * (yb - ya)));
        }

        public static void UkuleleLightning(NPC npc, int damage, int timesProcced)
        {
            npc.ROR().hasBeenStruckByUkuleleLightning = true;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Distance(npc, Main.npc[i]) <= 150 && Main.npc[i].ROR().hasBeenStruckByUkuleleLightning == false && !Main.npc[i].friendly && Main.npc[i].lifeMax > 5 && Main.npc[i].damage > 0)
                {
                    UkuleleLightning(Main.npc[i], damage, timesProcced + 1);

                    for (int j = 0; j < Distance(npc, Main.npc[i]); j++)
                    {
                        float angle = npc.Center.AngleTo(Main.npc[i].Center) - MathHelper.ToRadians(90);
                        if (LightningDrawPoints != null)
                        {
                            LightningDrawPoints.Add(npc.Center + new Vector2(0, j).RotatedBy(angle));
                        }
                    }
                }
            }

            NPC.HitInfo hit = new NPC.HitInfo
            {
                DamageType = DamageClass.Default,
                SourceDamage = damage,
                Damage = damage,
                Crit = false,
                Knockback = 0f,
                HitDirection = 0
            };
            npc.StrikeNPC(hit);
        }

        public static void TeslaLightning(Entity player, NPC npc, int damage)
        {
            if (npc.active)
            {
                for (int j = 0; j < Distance(player, npc); j++)
                {
                    float angle = player.Center.AngleTo(npc.Center) - MathHelper.ToRadians(90);
                    if (LightningDrawPoints != null)
                    {
                        LightningDrawPoints.Add(player.Center + new Vector2(0, j).RotatedBy(angle));
                    }
                }
            }

            NPC.HitInfo hit = new NPC.HitInfo
            {
                DamageType = DamageClass.Default,
                SourceDamage = damage,
                Damage = damage,
                Crit = false,
                Knockback = 0f,
                HitDirection = 0
            };
            npc.StrikeNPC(hit);
        }

        public override void Unload()
        {
            CountsAsBoss?.Clear();
            CountsAsBoss = null;
            RegisteredElites?.Clear();
            RegisteredElites = null;
        }

        public override void ResetEffects(NPC npc)
        {
            if (bleedShatterspleen && !npc.HasBuff<BleedingDebuff>())
            {
                bleedShatterspleen = false;
            }
            if (gasolineDamage > 0)
            {
                CheckGasoline(npc);
            }
            statDefense = 0;
            npc.ROR().npcSpeedStat = 1f;
            hasBeenStruckByUkuleleLightning = false;

            if (isAShielder)
            {
                maxShield = 0f;
                npc.lifeMax = regularMaxLife;
            }

            timeSinceLastHit++;
            isMending = false;
        }

        public void CheckGasoline(NPC npc)
        {
            for (int i = 0; i < NPC.maxBuffs; i++)
            {
                if (npc.buffTime[i] > 0 && npc.buffType[i] > 0 && Gasoline.FireDebuffsForGasolineDamageOverTime.Contains(npc.buffType[i]))
                {
                    return;
                }
            }
            gasolineDamage = 0;
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            convertToCursedFlames = false;
            regularMaxLife = npc.lifeMax;
        }

        public override bool PreAI(NPC npc)
        {
            //if (npc.HasBuff(ModContent.BuffType<ChronobaubleDebuff>()))
            //{
            //    if (chronobaubleToggle)
            //    {
            //        chronobaubleToggle = false;
            //        return false;
            //    }
            //    if (!chronobaubleToggle)
            //    {
            //        chronobaubleToggle = true;
            //    }
            //}

            if (npc.ROR().npcSpeedStat < 1f && npc.ROR().npcSpeedStat > 0f)
            {
                if (npcSpeedUpdate < (int)Math.Round(1 / npcSpeedStat))
                {
                    npcSpeedUpdate++;
                    return false;
                }
                else if (npcSpeedUpdate >= (int)Math.Round(1 / npcSpeedStat))
                {
                    npcSpeedUpdate = 0;
                }
            }

            if (npc.ROR().npcSpeedStat == 0f)
            {
                npc.velocity = Vector2.Zero;
                return false;
            }

            return true;
        }

        public override void PostAI(NPC npc)
        {
            if (convertToCursedFlames && npc.HasBuff(BuffID.OnFire))
            {
                int time = npc.buffTime[npc.FindBuffIndex(BuffID.OnFire)];
                npc.DelBuff(npc.FindBuffIndex(BuffID.OnFire));
                npc.AddBuff(BuffID.CursedInferno, time);
            }

            if (convertToCursedFlames && npc.HasBuff(BuffID.OnFire3))
            {
                int time = npc.buffTime[npc.FindBuffIndex(BuffID.OnFire3)];
                npc.DelBuff(npc.FindBuffIndex(BuffID.OnFire3));
                npc.AddBuff(BuffID.CursedInferno, time);
            }

            if (isAShielder)
            {
                npc.netUpdate = true;
                //makes sure shield doesnt go over your max shield
                shield = Math.Min(shield, maxShield);
                //sets your shield to max after not getting hit for a while
                if (maxShield > 0f && timeSinceLastHit >= 300)
                {
                    shield = maxShield;
                }

                //calculates how much health you can gain from a shield
                int add = (int)(regularMaxLife * shield);
                if (npc.life == npc.lifeMax)
                {
                    //adds extra health once (life does not decay)
                    npc.life += add;
                }
                //adds extra max life consistently, based on max shield rather than shield
                npc.lifeMax += (int)(regularMaxLife * maxShield); // was += add

                //adds regained shield upon hitting 6 seconds without damage
                if (shield > 0f && timeSinceLastHit == 300)
                {
                    //adds whichever is smaller- current life + potential health gain, or your max life (with shield)
                    npc.life = Math.Min(npc.life + (int)(regularMaxLife * shield), npc.lifeMax);
                }

                //if you take damage or lose health through wacky means
                if (savedLife > npc.life)
                {
                    //reset cooldown
                    timeSinceLastHit = 0;

                    //reduce shield by the amount of damage taken
                    if (shield > 0f)
                    {
                        shield = (float)Math.Max(shield - (savedLife - npc.life) / (float)regularMaxLife, 0f);
                        if (shield <= 0.01f)
                        {
                            shield = 0f;
                        }
                    }
                }
                savedLife = npc.life;
            }

            if (npc.netUpdate)
                Sync(npc.whoAmI);

            if (Main.dedServ)
                return;

            if (npc.HasBuff(ModContent.BuffType<BleedingDebuff>()))
            {
                if (Main.GameUpdateCount % 20 == 0)
                {
                    var d = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Blood, Main.rand.NextBool().ToDirectionInt(), -1f, 0, Scale: 1.5f);
                    d.velocity.X *= 0.5f;
                    SoundEngine.PlaySound(RiskOfTerrain.GetSounds("bleed_", 6, 0.3f, 0.1f, 0.25f), npc.Center);
                }
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            UpdateDebuffStack(npc, npc.HasBuff<BleedingDebuff>(), ref bleedingStacks, ref damage, 20, 16f);
            if (gasolineDamage > 0)
            {
                npc.lifeRegen -= gasolineDamage;
                if (damage < gasolineDamage)
                    damage = gasolineDamage / 8;
            }
        }

        public void UpdateDebuffStack(NPC npc, bool has, ref byte stacks, ref int damageNumbers, byte cap = 20, float dotMultiplier = 1f)
        {
            if (!has)
            {
                stacks = 0;
                return;
            }

            stacks = Math.Min(stacks, cap);
            int dot = (int)(stacks * dotMultiplier);

            if (dot >= 0)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= dot;
                if (damageNumbers < dot)
                    damageNumbers = dot / 8;
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            ModifyHit(npc, player, ref modifiers);
            mostRecentHitHadMask = player.ROR().accHappiestMask;
            if (mostRecentHitHadMask)
            {
                happiestMaskHolder = player.whoAmI;
            }
            else
            {
                happiestMaskHolder = -1;
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            ModifyHit(npc, projectile, ref modifiers);
            mostRecentHitHadMask = Main.player[projectile.owner].ROR().accHappiestMask;
            if (mostRecentHitHadMask)
            {
                happiestMaskHolder = projectile.owner;
            }
            else
            {
                happiestMaskHolder = -1;
            }
            lastHitProjectileType = projectile.type;
        }

        public void ModifyHit(NPC npc, Entity projOrPlayer, ref NPC.HitModifiers modifiers)
        {
            timeSinceLastHit = 0;

            modifiers.Defense *= npc.ROR().statDefense;

            if (npc.HasBuff<DeathMarkDebuff>())
                modifiers.SourceDamage *= 1.1f;

            if (shatterizationCount == 3)
            {
                modifiers.SourceDamage += 0.5f;
            }

            modifiers.SourceDamage += scorpionCount / 100;

            if (projOrPlayer is Projectile projectile)
            {
                if (Main.player[projectile.owner].ROR().accIgnitionTank)
                {
                    convertToCursedFlames = true;
                }
                else
                {
                    convertToCursedFlames = false;
                }
            }
            else if (projOrPlayer is Player player)
            {
                if (player.ROR().accIgnitionTank)
                {
                    convertToCursedFlames = true;
                }
                else
                {
                    convertToCursedFlames = false;
                }
            }

            if (npc.HasBuff(ModContent.BuffType<ArtiFreeze>()) && npc.life <= npc.lifeMax * 0.3)
            {
                NPC.HitInfo hit = new NPC.HitInfo();
                hit.Damage = npc.life;
                npc.StrikeNPC(hit);

                SoundStyle iceShatter = new SoundStyle();
                iceShatter.SoundPath = SoundID.Shatter.SoundPath;
                iceShatter.PitchVariance = 0.5f;
                iceShatter.Pitch = 0;
                iceShatter.Volume = 0.5f;
                SoundEngine.PlaySound(iceShatter);
            }
        }

        public override void OnKill(NPC npc)
        {
            var closest = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];

            if (mostRecentHitHadMask && Main.player[happiestMaskHolder].RollLuck(14) == 0 && !npc.GetGlobalNPC<GhostElite>().Active && !EliteNPCManager.EliteBlacklist.Contains(npc.type) && !npc.boss && !npc.friendly && npc.lifeMax > 5 && npc.damage > 0)
            {
                int i = NPC.NewNPC(npc.GetSource_Death(), (int)npc.Center.X, (int)npc.Center.Y, npc.type);
                Main.npc[i].GetGlobalNPC<GhostElite>().Active = true;
                Main.npc[i].GetGlobalNPC<GhostElite>().owner = happiestMaskHolder;
            }

            if (closest.ROR().minerSetBonusActive)
            {
                RORPlayer miner = closest.ROR();

                miner.minerFuel += 400;
                if (miner.minerFuel > 2000)
                {
                    miner.minerFuel = 2000;
                }
            }

            if (npc.SpawnedFromStatue || npc.friendly || npc.lifeMax < 5)
                return;

            var bb = new BitsByte(bleedShatterspleen);
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (npc.playerInteraction[i])
                {
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        Main.player[i].ROR().Accessories.OnKillEnemy(Main.player[i], new OnKillInfo()
                        {
                            type = npc.netID,
                            position = npc.position,
                            width = npc.width,
                            height = npc.height,
                            lifeMax = npc.lifeMax,
                            lastHitDamage = lastHitDamage,
                            miscInfo = bb,
                            value = npc.value,
                            friendly = npc.friendly,
                            spawnedFromStatue = npc.SpawnedFromStatue,
                            lastHitProjectile = npc.ROR().lastHitProjectileType,
                            aistyle = npc.aiStyle,
                            whoAmI = npc.whoAmI
                        });
                        continue;
                    }

                    var p = RiskOfTerrain.GetPacket(PacketType.OnKillEffect);
                    p.Write(i);
                    p.Write(npc.netID);
                    p.WriteVector2(npc.position);
                    p.Write(npc.width);
                    p.Write(npc.height);
                    p.Write(npc.lifeMax);
                    p.Write(lastHitDamage);
                    if (closest.whoAmI == i)
                    {
                        p.Write(bb);
                    }
                    else
                    {
                        p.Write((byte)0);
                    }
                    p.Write(npc.value);
                    p.Write(npc.friendly);
                    p.Write(npc.SpawnedFromStatue);
                    p.Write(npc.ROR().lastHitProjectileType);
                    p.Write(npc.aiStyle);
                    p.Write(npc.whoAmI);
                    p.Send(toClient: i);
                }
            }
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (ClientConfig.Instance.EnemyHBState != EnemyHealthBar.State.Vanilla)
            {
                drawConfused = npc.confused;
                npc.confused = false; // Disables the confused debuff from drawing
            }

            if (npc.HasBuff(ModContent.BuffType<CelestineInvis>()))
            {
                return false;
            }

            //if (npc.HasBuff(ModContent.BuffType<CelestineInvis>()))
            //{
            //    npc.alpha = 255;
            //}
            return true;
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (npc.HasBuff(ModContent.BuffType<RunaldFreeze>()) || npc.HasBuff(ModContent.BuffType<ArtiFreeze>()))
            {
                drawColor = Color.LightBlue;
            }

            if (npc.HasBuff(ModContent.BuffType<StunDebuff>()))
            {
                drawColor = Color.LightGray;
            }

            if (shatterizationCount > 0)
            {
                Color shatterDraw = new Color(drawColor.R, drawColor.G - (30 * shatterizationCount), drawColor.B - (30 * shatterizationCount));
                drawColor = shatterDraw;
            }
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (drawConfused)
            {
                npc.confused = true;
            }

            Texture2D texture = ModContent.Request<Texture2D>("RiskOfTerrain/Projectiles/Accessory/Damaging/UkuleleLightning").Value;

            if (LightningDrawPoints != null)
            {
                foreach (Vector2 pos in LightningDrawPoints)
                {
                    spriteBatch.Draw(texture, pos - screenPos, Color.LightBlue);
                }
                LightningDrawPoints.Clear();
            }
        }

        public void Send(int whoAmI, BinaryWriter writer)
        {
            writer.Write(bleedingStacks);

            var bb = new BitsByte(bleedShatterspleen, syncLifeMax, gasolineDamage > 0/*, shatterizationCount > 0*/);
            writer.Write(bb);

            if (bb[1])
            {
                writer.Write(Main.npc[whoAmI].lifeMax);
            }

            if (bb[2])
            {
                writer.Write(gasolineDamage);
            }

            //if (bb[3])
            //{
            //    writer.Write(shatterizationCount);
            //}

            Main.npc[whoAmI].GetElitePrefixes(out var prefixes);
            writer.Write((byte)prefixes.Count);
            for (int i = 0; i < prefixes.Count; i++)
            {
                writer.Write(prefixes[i].NetID);
            }
        }

        public void Receive(int whoAmI, BinaryReader reader)
        {
            bleedingStacks = reader.ReadByte();

            var bb = (BitsByte)reader.ReadByte();
            bleedShatterspleen = bb[0];

            if (bb[1])
            {
                Main.npc[whoAmI].lifeMax = reader.ReadInt32();
            }

            if (bb[2])
            {
                gasolineDamage = reader.ReadInt32();
            }

            //if (bb[3])
            //{
            //    shatterizationCount = reader.ReadInt32();
            //}

            int elitePrefixesCount = reader.ReadByte();
            for (int i = 0; i < elitePrefixesCount; i++)
            {
                Main.npc[whoAmI].GetGlobalNPC(RegisteredElites[reader.ReadByte()]).Active = true;
            }
        }

        public static void Sync(int npc)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                return;

            if (Main.npc[npc].TryGetGlobalNPC<RORNPC>(out var ror))
            {
                var p = RiskOfTerrain.GetPacket(PacketType.SyncRORNPC);
                p.Write(npc);
                ror.Send(npc, p);
                p.Send();
            }
        }
    }
}