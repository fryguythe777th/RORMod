using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Content.Artifacts
{
    public class ArtifactNPC : GlobalNPC
    {
        private MethodInfo NPC_GetHurtByOtherNPCs;

        public static HashSet<int> Soul_SpawnBlacklist { get; private set; }
        public static Dictionary<int, HashSet<int>> Chaos_HitBlacklist { get; private set; }

        public static NPCEquips EvolutionItems;

        public int npcParent;
        public int chaosHitDelay;

        public bool soulSpawn;
        public bool swarmsSpawn;

        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances => true;

        public ArtifactNPC()
        {
            npcParent = -1;
            chaosHitDelay = 0;
            swarmsSpawn = false;
        }

        public override void Load()
        {
            NPC_GetHurtByOtherNPCs = typeof(NPC).GetMethod("GetHurtByOtherNPCs", BindingFlags.NonPublic | BindingFlags.Instance);
            Soul_SpawnBlacklist = new HashSet<int>()
            {
                NPCID.DungeonSpirit,
            };
            Chaos_HitBlacklist = new Dictionary<int, HashSet<int>>()
            {
                [NPCID.PumpkingBlade] = new HashSet<int>() { NPCID.Pumpking, },
                [NPCID.BlueSlime] = new HashSet<int>() { NPCID.KingSlime, },
                [NPCID.SlimeSpiked] = new HashSet<int>() { NPCID.KingSlime, },
                [NPCID.KingSlime] = new HashSet<int>() { NPCID.BlueSlime, NPCID.SlimeSpiked, },
                [NPCID.QueenSlimeMinionBlue] = new HashSet<int>() { NPCID.QueenSlimeBoss, },
                [NPCID.QueenSlimeMinionPink] = new HashSet<int>() { NPCID.QueenSlimeBoss, },
                [NPCID.QueenSlimeMinionPurple] = new HashSet<int>() { NPCID.QueenSlimeBoss, },
                [NPCID.QueenSlimeBoss] = new HashSet<int>() { NPCID.QueenSlimeMinionBlue, NPCID.QueenSlimeMinionPink, NPCID.QueenSlimeMinionPurple, },
                [NPCID.SkeletronHand] = new HashSet<int>() { NPCID.SkeletronHead, },
                [NPCID.SkeletronHead] = new HashSet<int>() { NPCID.SkeletronHand, },
                [NPCID.SkeletronPrime] = new HashSet<int>() { NPCID.SkeletronPrime, NPCID.PrimeCannon, NPCID.PrimeLaser, NPCID.PrimeSaw, NPCID.PrimeVice, },
                [NPCID.PrimeCannon] = new HashSet<int>() { NPCID.SkeletronPrime, NPCID.PrimeCannon, NPCID.PrimeLaser, NPCID.PrimeSaw, NPCID.PrimeVice, },
                [NPCID.PrimeLaser] = new HashSet<int>() { NPCID.SkeletronPrime, NPCID.PrimeCannon, NPCID.PrimeLaser, NPCID.PrimeSaw, NPCID.PrimeVice, },
                [NPCID.PrimeSaw] = new HashSet<int>() { NPCID.SkeletronPrime, NPCID.PrimeCannon, NPCID.PrimeLaser, NPCID.PrimeSaw, NPCID.PrimeVice, },
                [NPCID.PrimeVice] = new HashSet<int>() { NPCID.SkeletronPrime, NPCID.PrimeCannon, NPCID.PrimeLaser, NPCID.PrimeSaw, NPCID.PrimeVice, },
                [NPCID.Plantera] = new HashSet<int>() { NPCID.Plantera, NPCID.PlanterasHook, NPCID.PlanterasTentacle, },
                [NPCID.PlanterasHook] = new HashSet<int>() { NPCID.Plantera, NPCID.PlanterasHook, NPCID.PlanterasTentacle, },
                [NPCID.PlanterasTentacle] = new HashSet<int>() { NPCID.Plantera, NPCID.PlanterasHook, NPCID.PlanterasTentacle, },
                [NPCID.Golem] = new HashSet<int>() { NPCID.Golem, NPCID.GolemFistLeft, NPCID.GolemFistRight, },
                [NPCID.GolemFistRight] = new HashSet<int>() { NPCID.Golem, NPCID.GolemFistLeft, NPCID.GolemFistRight, },
                [NPCID.GolemFistLeft] = new HashSet<int>() { NPCID.Golem, NPCID.GolemFistLeft, NPCID.GolemFistRight, },
                [NPCID.MoonLordCore] = new HashSet<int>() { NPCID.MoonLordCore, NPCID.MoonLordHead, NPCID.MoonLordHand, NPCID.MoonLordFreeEye, NPCID.MoonLordLeechBlob },
                [NPCID.PirateShip] = new HashSet<int>() { NPCID.PirateShip, NPCID.PirateShipCannon, },
                [NPCID.PirateShipCannon] = new HashSet<int>() { NPCID.PirateShip, NPCID.PirateShipCannon, },
                [NPCID.Creeper] = new HashSet<int>() { NPCID.Creeper, },
                [NPCID.EaterofWorldsHead] = new HashSet<int>() { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail, },
                [NPCID.TheDestroyer] = new HashSet<int>() { NPCID.TheDestroyer, NPCID.TheDestroyerBody, NPCID.TheDestroyerTail, },
                [NPCID.TheDestroyerBody] = new HashSet<int>() { NPCID.TheDestroyer, NPCID.TheDestroyerBody, NPCID.TheDestroyerTail, },
                [NPCID.TheDestroyerTail] = new HashSet<int>() { NPCID.TheDestroyer, NPCID.TheDestroyerBody, NPCID.TheDestroyerTail, },
                [NPCID.MartianSaucer] = new HashSet<int>() { NPCID.MartianSaucer, NPCID.MartianSaucerCannon, NPCID.MartianSaucerCore, NPCID.MartianSaucerTurret, },
                [NPCID.MartianSaucerCannon] = new HashSet<int>() { NPCID.MartianSaucer, NPCID.MartianSaucerCannon, NPCID.MartianSaucerCore, NPCID.MartianSaucerTurret, },
                [NPCID.MartianSaucerCore] = new HashSet<int>() { NPCID.MartianSaucer, NPCID.MartianSaucerCannon, NPCID.MartianSaucerCore, NPCID.MartianSaucerTurret, },
                [NPCID.MartianSaucerTurret] = new HashSet<int>() { NPCID.MartianSaucer, NPCID.MartianSaucerCannon, NPCID.MartianSaucerCore, NPCID.MartianSaucerTurret, },
                [NPCID.ForceBubble] = new HashSet<int>() { NPCID.ForceBubble, NPCID.MartianOfficer, },
                [NPCID.MartianOfficer] = new HashSet<int>() { NPCID.ForceBubble, NPCID.MartianOfficer, },
            };
        }

        public override void Unload()
        {
            Soul_SpawnBlacklist?.Clear();
            Soul_SpawnBlacklist = null;
            Chaos_HitBlacklist?.Clear();
            Chaos_HitBlacklist = null;
            NPC_GetHurtByOtherNPCs = null;
        }

        public override void SetDefaults(NPC npc)
        {
            npcParent = -1;
            chaosHitDelay = 0;
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (source is EntitySource_RevengeSystem || source is EntitySource_Sync)
            {
                swarmsSpawn = true;
                return;
            }
            if (source is EntitySource_Parent parent && parent.Entity is NPC)
            {
                npcParent = parent.Entity.whoAmI;
            }
        }

        public override bool? CanHitNPC(NPC npc, NPC target)
        {
            return ArtifactSystem.chaos && Chaos_CanDamageOtherNPC(npc, target) ? null : false;
        }

        public bool Chaos_CanDamageOtherNPC(NPC npc, NPC target)
        {
            return npc.whoAmI != target.whoAmI && !(Chaos_HitBlacklist.TryGetValue(target.netID, out var l) && l.Contains(npc.netID)) &&
                target.aiStyle != NPCAIStyleID.Worm &&
                target.realLife != npc.whoAmI && target.whoAmI != npc.realLife && npc.realLife != target.realLife;
        }

        public override void PostAI(NPC npc)
        {
            if (npcParent != -1 && !Main.npc[npcParent].active)
            {
                npcParent = -1;
            }
            if (ArtifactSystem.chaos)
            {
                if (chaosHitDelay < 30)
                {
                    chaosHitDelay++;
                    return;
                }
                NPC_GetHurtByOtherNPCs.Invoke(npc, new object[] { NPCID.Sets.AllNPCs });
            }
        }

        public override bool SpecialOnKill(NPC npc)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                var s = npc.GetSource_Death();
                if (RORMod.soul && !npc.SpawnedFromStatue && !soulSpawn)
                {
                    int amt = Main.masterMode ? 3 : Main.expertMode ? 2 : 1;
                    if (Main.getGoodWorld)
                    {
                        amt *= 2;
                    }
                    var p = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];
                    for (int i = 0; i < amt; i++)
                    {
                        var n = NPC.NewNPCDirect(s, npc.Center, SoulArtifact_ChooseNPCToSpawn(npc, p));
                        n.velocity = Main.rand.NextVector2Unit() * 8f;
                        n.SpawnedFromStatue = true;
                        n.GetGlobalNPC<ArtifactNPC>().npcParent = npcParent;
                        n.GetGlobalNPC<ArtifactNPC>().soulSpawn = true;
                        n.netUpdate = true;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n.whoAmI);
                        }
                    }
                }
                if (RORMod.spite)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        int p = Projectile.NewProjectile(s, npc.Center, Main.rand.NextVector2Unit() * 8f, ProjectileID.BombSkeletronPrime, 80, 1f, Main.myPlayer);
                        Main.projectile[p].timeLeft *= 2;
                    }
                }
            }
            return false;
        }
        public int SoulArtifact_ChooseNPCToSpawn(NPC npc, Player player)
        {
            if (player.ZoneDungeon)
            {
                return Main.hardMode && Main.rand.NextBool() ? NPCID.AngryBones : NPCID.Skeleton;
            }
            if (player.ZoneCrimson)
            {
                if (npc.wet && Main.rand.NextBool())
                {
                    return Main.hardMode && Main.rand.NextBool() ? NPCID.BloodFeeder : NPCID.CrimsonGoldfish;
                }
                if (Main.rand.NextBool())
                {
                    return player.ZoneSnow ? NPCID.CrimsonPenguin : NPCID.CrimsonBunny;
                }
                return Main.hardMode && Main.rand.NextBool() ? NPCID.Herpling : NPCID.BloodCrawler;
            }
            if (player.ZoneCorrupt)
            {
                if (npc.wet && Main.rand.NextBool())
                {
                    return NPCID.CorruptGoldfish;
                }
                if (Main.rand.NextBool())
                {
                    return player.ZoneSnow ? NPCID.CorruptPenguin : NPCID.CorruptBunny;
                }
                return Main.hardMode && Main.rand.NextBool() ? NPCID.Corruptor : NPCID.EaterofSouls;
            }
            if (player.ZoneJungle)
            {
                if (npc.wet && Main.rand.NextBool())
                {
                    return Main.hardMode && Main.rand.NextBool() ? NPCID.Arapaima : Main.rand.NextBool() ? NPCID.AnglerFish : NPCID.Piranha;
                }
                var l = new List<int>() { NPCID.JungleSlime, };
                if (npc.position.Y > Main.worldSurface * 16f)
                {
                    l.Add(NPCID.Hornet);
                    if (Main.hardMode)
                        l.Add(NPCID.MossHornet);
                }
                return Main.rand.Next(l);
            }
            if (player.ZoneBeach && npc.wet)
            {
                return Main.hardMode && Main.rand.NextBool() ? NPCID.GreenJellyfish : NPCID.PinkJellyfish;
            }
            if (player.ZoneUnderworldHeight)
            {
                return Main.hardMode && NPC.downedMechBossAny && Main.rand.NextBool() ? NPCID.Lavabat : NPCID.Hellbat;
            }
            if (npc.wet && Main.rand.NextBool())
            {
                return Main.hardMode && Main.rand.NextBool() ? NPCID.AnglerFish : NPCID.Piranha;
            }
            if (player.ZoneHallow && Main.hardMode)
            {
                return Main.hardMode && Main.rand.NextBool() ? NPCID.Gastropod : NPCID.Pixie;
            }
            if (player.ZoneSnow)
            {
                if (npc.position.Y > Main.worldSurface * 16f)
                    return Main.hardMode && Main.rand.NextBool() ? NPCID.IceElemental : NPCID.IceBat;
                return Main.hardMode && Main.rand.NextBool() ? NPCID.IceElemental : NPCID.IceSlime;
            }
            if (player.ZoneDesert)
            {
                if (npc.position.Y > Main.worldSurface * 16f)
                    return Main.hardMode && Main.rand.NextBool() ? NPCID.DesertBeast : NPCID.SandSlime;
                return Main.hardMode && Main.rand.NextBool() ? NPCID.Mummy : NPCID.Vulture;
            }
            if (npc.position.Y > Main.worldSurface * 16f)
            {
                return Main.hardMode && Main.rand.NextBool() ? Main.rand.NextFloat(1f) < 0.1f ? NPCID.SkeletonArcher : NPCID.ArmoredSkeleton : NPCID.Skeleton;
            }
            if (!Main.dayTime)
            {
                return Main.hardMode && Main.rand.NextBool() ? NPCID.PossessedArmor : NPCID.Zombie;
            }
            return NPCID.BlueSlime;
        }

        public static NPCEquips RollEvolutionItemsSet()
        {
            return new NPCEquips(0);
        }
    }
}
