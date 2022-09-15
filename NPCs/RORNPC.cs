using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RORMod.Buffs.Debuff;
using RORMod.Content.Elites;
using RORMod.Items.Accessories;
using RORMod.Projectiles.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.NPCs
{
    public class RORNPC : GlobalNPC
    {
        public byte bleedingStacks;
        public bool bleedShatterspleen;

        public override bool InstancePerEntity => true;

        public static List<EliteNPC> RegisteredElites { get; private set; }

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
            RegisteredElites = new List<EliteNPC>();
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
            if (!npc.HasBuff<BleedingDebuff>())
            {
                bleedShatterspleen = false;
            }
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
                    var d =Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Blood, Main.rand.NextBool().ToDirectionInt(), -1f, 0, Scale: 1.5f);
                    d.velocity.X *= 0.5f;
                    SoundEngine.PlaySound(RORMod.GetSounds("bleed_", 6, 0.3f, 0.1f, 0.25f), npc.Center);
                }
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            UpdateDebuffStack(npc, npc.HasBuff<BleedingDebuff>(), ref bleedingStacks, ref damage, 20, 16f);
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

        public override void ModifyTypeName(NPC npc, ref string typeName)
        {
            string prefixes = "";
            npc.GetElitePrefixes(out var list);
            foreach (var e in list)
            {
                if (!string.IsNullOrEmpty(prefixes))
                    prefixes += ", ";
                prefixes += e.Prefix;
            }
            typeName = prefixes + " " + typeName;
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

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            CheckWarbannerBossProgress(npc, damage);
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            CheckWarbannerBossProgress(npc, damage);
        }

        public void CheckWarbannerBossProgress(NPC npc, int damage)
        {
            if (!npc.boss)
                return;

            float percent = npc.life / (float)npc.lifeMax;
            float lastPercent = (npc.life + damage) / (float)npc.lifeMax;
            if ((percent <= 0.25f && lastPercent > 0.25f) || (percent <= 0.5f && lastPercent > 0.5f) || (percent <= 0.75f && lastPercent > 0.75f) || (percent < 1f && lastPercent >= 1f))
            {
                var closest = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];
                if (!closest.dead)
                {
                    var ror = closest.ROR();
                    if (ror.accWarbanner != null)
                        ror.SpawnWarbanner();
                }
            }
        }

        public override void OnKill(NPC npc)
        {
            if (npc.SpawnedFromStatue || npc.friendly || npc.lifeMax < 5)
                return;

            var closest = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];
            var ror = closest.ROR();
            if (bleedShatterspleen)
            {
                Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, npc.DirectionFrom(closest.Center) * 0.1f, 
                    ModContent.ProjectileType<ShatterspleenExplosion>(), npc.lifeMax / 4, 6f, closest.whoAmI);
            }
            if (ror.accMonsterTooth != null)
            {
                Projectile.NewProjectile(closest.GetSource_Accessory(ror.accMonsterTooth), npc.Center, new Vector2(0f, -2f), ModContent.ProjectileType<HealingOrb>(), 0, 0, closest.whoAmI);
            }
            if (ror.accTopazBrooch)
            {
                ror.barrier = Math.Min(ror.barrier + 15f / closest.statLifeMax2, 1f);
                closest.statLife += 15;
            }
            if (ror.accWarbanner != null)
            {
                ror.warbannerProgress_Enemies++;
                //Main.NewText(ror.warbannerProgress_Enemies);
            }
            if (ror.accFireworks)
            {
                Projectile.NewProjectile(closest.GetSource_FromThis(), closest.Center, Vector2.Zero, ModContent.ProjectileType<FireworksProjectile>(), Math.Clamp((int)(npc.lifeMax * 0.05), 20, 80), 0, closest.whoAmI, 40);
            }
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            npc.GetElitePrefixes(out var list);
            foreach (var e in list)
            {
                var npcE = npc.GetGlobalNPC(e);
                if (npcE.Active)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.Transform);
                    npcE.Shader.Apply(npc);
                    break;
                }
            }
            return true;
        }

        public void Send(int whoAmI, BinaryWriter writer)
        {
            writer.Write(bleedingStacks);

            var bb = new BitsByte(bleedShatterspleen);
            writer.Write(bb);
        }

        public void Receive(int whoAmI, BinaryReader reader)
        {
            bleedingStacks = reader.ReadByte();

            var bb = (BitsByte)reader.ReadByte();
            bleedShatterspleen = bb[0];
        }

        public static void Sync(int npc)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                return;

            if (Main.npc[npc].TryGetGlobalNPC<RORNPC>(out var ror))
            {
                var p = RORMod.GetPacket(PacketType.SyncRORNPC);
                p.Write(npc);
                ror.Send(npc, p);
                p.Send();
            }
        }
    }
}