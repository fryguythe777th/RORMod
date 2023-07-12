using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Buffs.WakeOfVultures;
using RiskOfTerrain.Items.Accessories.T1Common;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs
{
    public class RORBuff : GlobalBuff
    {
        public static int[] BottledChaosOptions = new int[77];
        public static Asset<Texture2D>[] BottledChaosTextures = new Asset<Texture2D>[77];

        public override void Load()
        {
            BottledChaosOptions = new int[]
            {
                BuffID.AmmoReservation,
                BuffID.Archery,
                BuffID.Builder,
                BuffID.Calm,
                BuffID.Crate,
                BuffID.Dangersense,
                BuffID.Endurance,
                BuffID.Featherfall,
                BuffID.Fishing,
                BuffID.Flipper,
                BuffID.Gills,
                BuffID.Gravitation,
                BuffID.Heartreach,
                BuffID.Hunter,
                BuffID.Inferno,
                BuffID.Invisibility,
                BuffID.Ironskin,
                BuffID.Lifeforce,
                BuffID.Lucky,
                BuffID.MagicPower,
                BuffID.ManaRegeneration,
                BuffID.Mining,
                BuffID.NightOwl,
                BuffID.ObsidianSkin,
                BuffID.Rage,
                BuffID.Regeneration,
                BuffID.Shine,
                BuffID.Sonar,
                BuffID.Spelunker,
                BuffID.Summoning,
                BuffID.Swiftness,
                BuffID.Thorns,
                BuffID.Titan,
                BuffID.Warmth,
                BuffID.WaterWalking,
                BuffID.Wrath,
                BuffID.WellFed,
                BuffID.WellFed2,
                BuffID.WellFed3,
                BuffID.WeaponImbueConfetti,
                BuffID.WeaponImbueCursedFlames,
                BuffID.WeaponImbueFire,
                BuffID.WeaponImbueGold,
                BuffID.WeaponImbueIchor,
                BuffID.WeaponImbueNanites,
                BuffID.WeaponImbuePoison,
                BuffID.WeaponImbueVenom,
                BuffID.BeetleEndurance1,
                BuffID.BeetleMight1,
                BuffID.NebulaUpDmg1,
                BuffID.NebulaUpLife1,
                BuffID.NebulaUpMana1,
                BuffID.SwordWhipPlayerBuff,
                BuffID.ScytheWhipPlayerBuff,
                BuffID.IceBarrier,
                BuffID.ThornWhipPlayerBuff,
                BuffID.Merfolk,
                BuffID.Panic,
                BuffID.RapidHealing,
                BuffID.TitaniumStorm,
                BuffID.Werewolf,
                BuffID.SugarRush,
                BuffID.Campfire,
                BuffID.DryadsWard,
                BuffID.Sunflower,
                BuffID.HeartLamp,
                BuffID.Honey,
                BuffID.PeaceCandle,
                BuffID.StarInBottle,
                BuffID.CatBast,

                ModContent.BuffType<BandolierBuff>(),
                ModContent.BuffType<BerzerkerBuff>(),
                ModContent.BuffType<HuntersHarpoonBuff>(),
                ModContent.BuffType<PredatoryInstinctsBuff>(),
                ModContent.BuffType<WarHornBuff>(),
                ModContent.BuffType<BlazingWOV>(),
                ModContent.BuffType<CelestineWOV>(),
                ModContent.BuffType<OverloadingWOV>(),
            };

            BottledChaosTextures = new Asset<Texture2D>[]
            {
                TextureAssets.Buff[BuffID.AmmoReservation],
                TextureAssets.Buff[BuffID.Archery],
                TextureAssets.Buff[BuffID.Builder],
                TextureAssets.Buff[BuffID.Calm],
                TextureAssets.Buff[BuffID.Crate],
                TextureAssets.Buff[BuffID.Dangersense],
                TextureAssets.Buff[BuffID.Endurance],
                TextureAssets.Buff[BuffID.Featherfall],
                TextureAssets.Buff[BuffID.Fishing],
                TextureAssets.Buff[BuffID.Flipper],
                TextureAssets.Buff[BuffID.Gills],
                TextureAssets.Buff[BuffID.Gravitation],
                TextureAssets.Buff[BuffID.Heartreach],
                TextureAssets.Buff[BuffID.Hunter],
                TextureAssets.Buff[BuffID.Inferno],
                TextureAssets.Buff[BuffID.Invisibility],
                TextureAssets.Buff[BuffID.Ironskin],
                TextureAssets.Buff[BuffID.Lifeforce],
                TextureAssets.Buff[BuffID.Lucky],
                TextureAssets.Buff[BuffID.MagicPower],
                TextureAssets.Buff[BuffID.ManaRegeneration],
                TextureAssets.Buff[BuffID.Mining],
                TextureAssets.Buff[BuffID.NightOwl],
                TextureAssets.Buff[BuffID.ObsidianSkin],
                TextureAssets.Buff[BuffID.Rage],
                TextureAssets.Buff[BuffID.Regeneration],
                TextureAssets.Buff[BuffID.Shine],
                TextureAssets.Buff[BuffID.Sonar],
                TextureAssets.Buff[BuffID.Spelunker],
                TextureAssets.Buff[BuffID.Summoning],
                TextureAssets.Buff[BuffID.Swiftness],
                TextureAssets.Buff[BuffID.Thorns],
                TextureAssets.Buff[BuffID.Titan],
                TextureAssets.Buff[BuffID.Warmth],
                TextureAssets.Buff[BuffID.WaterWalking],
                TextureAssets.Buff[BuffID.Wrath],
                TextureAssets.Buff[BuffID.WellFed],
                TextureAssets.Buff[BuffID.WellFed2],
                TextureAssets.Buff[BuffID.WellFed3],
                TextureAssets.Buff[BuffID.WeaponImbueConfetti],
                TextureAssets.Buff[BuffID.WeaponImbueCursedFlames],
                TextureAssets.Buff[BuffID.WeaponImbueFire],
                TextureAssets.Buff[BuffID.WeaponImbueGold],
                TextureAssets.Buff[BuffID.WeaponImbueIchor],
                TextureAssets.Buff[BuffID.WeaponImbueNanites],
                TextureAssets.Buff[BuffID.WeaponImbuePoison],
                TextureAssets.Buff[BuffID.WeaponImbueVenom],
                TextureAssets.Buff[BuffID.BeetleEndurance1],
                TextureAssets.Buff[BuffID.BeetleMight1],
                TextureAssets.Buff[BuffID.NebulaUpDmg1],
                TextureAssets.Buff[BuffID.NebulaUpLife1],
                TextureAssets.Buff[BuffID.NebulaUpMana1],
                TextureAssets.Buff[BuffID.SwordWhipPlayerBuff],
                TextureAssets.Buff[BuffID.ScytheWhipPlayerBuff],
                TextureAssets.Buff[BuffID.IceBarrier],
                TextureAssets.Buff[BuffID.ThornWhipPlayerBuff],
                TextureAssets.Buff[BuffID.Merfolk],
                TextureAssets.Buff[BuffID.Panic],
                TextureAssets.Buff[BuffID.RapidHealing],
                TextureAssets.Buff[BuffID.TitaniumStorm],
                TextureAssets.Buff[BuffID.Werewolf],
                TextureAssets.Buff[BuffID.SugarRush],
                TextureAssets.Buff[BuffID.Campfire],
                TextureAssets.Buff[BuffID.DryadsWard],
                TextureAssets.Buff[BuffID.Sunflower],
                TextureAssets.Buff[BuffID.HeartLamp],
                TextureAssets.Buff[BuffID.Honey],
                TextureAssets.Buff[BuffID.PeaceCandle],
                TextureAssets.Buff[BuffID.StarInBottle],
                TextureAssets.Buff[BuffID.CatBast],

                ModContent.Request<Texture2D>("RiskOfTerrain/Buffs/BandolierBuff"),
                ModContent.Request<Texture2D>("RiskOfTerrain/Buffs/BerzerkerBuff"),
                ModContent.Request<Texture2D>("RiskOfTerrain/Buffs/HuntersHarpoonBuff"),
                ModContent.Request<Texture2D>("RiskOfTerrain/Buffs/PredatoryInstinctsBuff"),
                ModContent.Request<Texture2D>("RiskOfTerrain/Buffs/WarHornBuff"),
                ModContent.Request<Texture2D>("RiskOfTerrain/Buffs/WakeOfVultures/BlazingWOV"),
                ModContent.Request<Texture2D>("RiskOfTerrain/Buffs/WakeOfVultures/CelestineWOV"),
                ModContent.Request<Texture2D>("RiskOfTerrain/Buffs/WakeOfVultures/OverloadingWOV"),
            };
        }

        public override void Unload()
        {
            BottledChaosOptions = null;
            BottledChaosTextures = null;
        }
    }
}