using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using RiskOfTerrain.Items.Accessories.T3Legendary;
using RiskOfTerrain.Items.Accessories.T1Common;
using RiskOfTerrain.Items.CharacterSets.Artificer;
using RiskOfTerrain.Items.Accessories.T2Uncommon;

namespace RiskOfTerrain.NPCs
{
    public class ItemLootNPC : GlobalNPC
    {
        //general rules: 10%/20% (10/5) white item, 2.5%/5% (40/20) green item, 0.5%/1% (200/100) red item.
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.BloodZombie || npc.type == NPCID.Drippler)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<MonsterTooth>(), 149, 75)); //this one is specific, to match shark tooth necklace drop rates 
            }
            if (npc.type == NPCID.Nurse)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Medkit>(), 8)); //this one matches town npc loot patterns
            }
            if (npc.type == NPCID.DD2DarkMageT1)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Warbanner>(), 2, 1));
            }
            if (npc.type == NPCID.DD2DarkMageT3)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<FrostRelic>(), 8, 4));
            }
            if (npc.type == NPCID.Harpy)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<HopooFeather>(), 10, 5));
            }
            if (npc.type == NPCID.GoblinThief)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Crowbar>(), 10, 5));
            }
            if (npc.type == NPCID.DemonEye || npc.type == NPCID.DemonEye2)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<LensMakersGlasses>(), 200, 100));
            }

            if (npc.type == NPCID.Wraith)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<HarvestersScythe>(), 40, 20));
            }
            if (npc.type == NPCID.RuneWizard)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DeathMark>(), 1));
            }

            if (npc.type == NPCID.Vulture)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<WakeOfVultures>(), 200, 100));
            }
            if (npc.type == NPCID.DesertScorpionWalk || npc.type == NPCID.DesertScorpionWall)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<SymbioticScorpion>(), 200, 100));
            }
            if (npc.type == NPCID.IceSlime)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<FrostRelic>(), 200, 100));
            }
            if (npc.type == NPCID.BrainofCthulhu)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<SentientMeatHook>(), 8, 4));
            }
            if (System.Array.IndexOf(new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail }, npc.type) > -1)
            {
                LeadingConditionRule leadingConditionRule = new(new Conditions.LegacyHack_IsABoss());
                leadingConditionRule.OnSuccess(ItemDropRule.NormalvsExpert(ModContent.ItemType<AlienHead>(), 8, 4));
                npcLoot.Add(leadingConditionRule);
            }
            if (npc.type == NPCID.Necromancer || npc.type == NPCID.NecromancerArmored)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<HappiestMask>(), 40, 20));
            }
            if (npc.type == NPCID.GoblinSummoner) 
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<BottledChaos>(), 6, 3));
            }
            if (npc.type == NPCID.BigMimicCorruption) 
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Brainstalks>(), 5));
            }
            if (npc.type == NPCID.BigMimicCrimson)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BerzerkersPauldron>(), 5));
            }
        }

        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Mechanic)
            {
                shop.Add(ModContent.ItemType<ArtificerHead>(), Condition.NightOrEclipse);
                shop.Add(ModContent.ItemType<ArtificerBody>(), Condition.NightOrEclipse);
                shop.Add(ModContent.ItemType<ArtificerLegs>(), Condition.NightOrEclipse);
                shop.Add(ModContent.ItemType<ArtificerBoltWeapon>(), Condition.NightOrEclipse);
            }
            if (shop.NpcType == NPCID.ArmsDealer)
            {
                shop.Add(ModContent.ItemType<ArmorPiercingRounds>(), Condition.DownedEowOrBoc);
                shop.Add(ModContent.ItemType<BackupMagazine>());
            }
            if (shop.NpcType == NPCID.PartyGirl)
            {
                shop.Add(ModContent.ItemType<BundleOfFireworks>());
            }
            if (shop.NpcType == NPCID.BestiaryGirl)
            {
                shop.Add(ModContent.ItemType<RedWhip>());
            }
            if (shop.NpcType == NPCID.Dryad)
            {
                shop.Add(ModContent.ItemType<LeechingSeed>(), Condition.DownedEowOrBoc);
            }
            if (shop.NpcType == NPCID.Demolitionist)
            {
                shop.Add(ModContent.ItemType<StickyExplosives>(), Condition.DownedEyeOfCthulhu);
                shop.Add(ModContent.ItemType<BrilliantBehemoth>(), Condition.Hardmode);
            }
            if (shop.NpcType == NPCID.Cyborg)
            {
                shop.Add(ModContent.ItemType<ICBM>());
            }
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add(ModContent.ItemType<RollOfPennies>());
            }
            if (shop.NpcType == NPCID.SkeletonMerchant)
            {
                shop.Add(ModContent.ItemType<SoulboundCatalyst>(), Condition.Hardmode, Condition.MoonPhasesEven);
            }
        }
    }
}

/*
 * ITEMS WITHOUT ALT OBTAINMENT:
 * Bustling Fungus - TILE
 * Focus Crystal - TILE
 * Gasoline - OBTAINED W/ TAR LATER
 * Oddly Shaped Opal
 * Paul's Goat Hoof
 * Personal Shield Generator
 * Repulsion Armor Plate
 * Soldier's Syringe
 * Stun Grenade
 * Tougher Times
 * Tri Tip Dagger
 * 
 * AtG
 * Bandolier
 * Chronobauble
 * Fuel Cell
 * Ghor's Tome
 * Hunter's Harpoon
 * Ignition Tank
 * Infusion
 * Kjaro's Band - ELDER LEMURIANS
 * Lepton Daisy - TILE
 * Predatory Instincts
 * Razorwire
 * Rose Buckler
 * Runald's Band - ELDER LEMURIANS
 * Ukulele
 * War Horn
 * Will o' the Wisp - WISPS
 * 
 * Ben's Raincoat
 * Clover - TILE
 * Dio's Best Friend - CELESTINES?
 * Hardlight Afterburner
 * Headset
 * Interstellar Desk Plant - TILE
 * Lazer Scope
 * N'kuhana's Opinion - MALACHITES?
 * Rejuvenation Rack - MENDINGS?
 * Resonance Disc
 * Sacrificial Dagger
 * Unstable Tesla Coil - OVERLOADINGS?
 * 
 * Under elites dropping red items model, some would be from ror1 (fireman boots => blazing)
*/
