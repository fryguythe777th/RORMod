using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Content;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Content.Elites;
using RiskOfTerrain.Items.Accessories.T1Common;
using RiskOfTerrain.Items.Consumable;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.NPCs
{
    public class RORNPC : GlobalNPC
    {
        public byte bleedingStacks;
        public bool bleedShatterspleen;
        public bool syncLifeMax;

        public int lastHitDamage;
        public int gasolineDamage;

        public int statDefense;

        private bool drawConfused;

        public override bool InstancePerEntity => true;

        public static List<EliteNPCBase> RegisteredElites { get; private set; }

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
            On.Terraria.NPC.checkArmorPenetration += NPC_checkArmorPenetration;
            On.Terraria.NPC.StrikeNPC += NPC_StrikeNPC;
        }

        private static double NPC_StrikeNPC(On.Terraria.NPC.orig_StrikeNPC orig, NPC self, int Damage, float knockBack, int hitDirection, bool crit, bool noEffect, bool fromNet)
        {
            if (self.TryGetGlobalNPC<RORNPC>(out var ror))
            {
                ror.lastHitDamage = Damage;
            }
            return orig(self, Damage, knockBack, hitDirection, crit, noEffect, fromNet);
        }

        public static int NPC_checkArmorPenetration(On.Terraria.NPC.orig_checkArmorPenetration orig, NPC self, int armorPenetration)
        {
            armorPenetration -= self.ROR().statDefense;
            return orig(self, armorPenetration);
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

        public override void PostAI(NPC npc)
        {
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

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            ModifiyHit(npc, ref damage, ref knockback, ref crit);
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            ModifiyHit(npc, ref damage, ref knockback, ref crit);
        }

        public void ModifiyHit(NPC npc, ref int damage, ref float knockback, ref bool crit)
        {
            if (npc.HasBuff<DeathMarkDebuff>())
                damage += (int)(damage * 0.1f);
        }

        public override void OnKill(NPC npc)
        {
            if (npc.SpawnedFromStatue || npc.friendly || npc.lifeMax < 5)
                return;

            var closest = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];

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
                            spawnedFromStatue = npc.SpawnedFromStatue
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
            return true;
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (drawConfused)
            {
                npc.confused = true;
            }
        }

        public void Send(int whoAmI, BinaryWriter writer)
        {
            writer.Write(bleedingStacks);

            var bb = new BitsByte(bleedShatterspleen, syncLifeMax, gasolineDamage > 0);
            writer.Write(bb);

            if (bb[1])
            {
                writer.Write(Main.npc[whoAmI].lifeMax);
            }

            if (bb[2])
            {
                writer.Write(gasolineDamage);
            }

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