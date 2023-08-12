using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Content;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Content.Elites;
using RiskOfTerrain.Items.Accessories.T1Common;
using RiskOfTerrain.Items.Consumable;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace RiskOfTerrain.NPCs
{
    public class CautiousSlugCritter : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Snail];
            //BRUH HOW MANY FRAMES DO THE SNAIL GOT
            //AT LEAST >5
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("Mods.RiskOfTerrain.Bestiary.CautiousSlugCritter"),
            });
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Snail);
            NPC.aiStyle = NPCAIStyleID.Snail;
            NPC.catchItem = ModContent.ItemType<CautiousSlug>();
            NPC.width = 30;
            NPC.height = 20;
            AnimationType = NPCID.Snail;
            NPC.friendly = true;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneNormalUnderground)
            {
                return SpawnCondition.Underground.Chance * 0.05f;
            }
            else if (spawnInfo.Player.ZoneNormalCaverns)
            {
                return SpawnCondition.Cavern.Chance * 0.1f;
            }
            else
            {
                return 0f;
            }
        }
    }
}