using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.NPCs;
using RiskOfTerrain.Projectiles.Elite;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.Aspects
{
    public class TrollAspect : GenericAspect
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight(Item.Center, TorchID.Yellow);
        }
    }
}

/*
 * FILES THAT HAVE BEEN DISABLED
MendingAspect
AlienHead
BensRaincoat
HappiestMask
HardlightAfterburner
Headset
NkuhanasOpinion
RejuvenationRack
ResonanceDisc
SoulboundCatalyst
UnstableTeslaCoil
ArtificerLegs
ArtificerHead
ArtificerBoltWeapon
ArtificerBody
CommandoLegs
CommandoHead
CommandoGunWeapon
CommandoGrenadeWeapon
CommandoBody
MinerPickaxeWeapon
MinerHead
MinerLegs
MinerBody
CautiousSlugCritter
CautiousSlug
*/

/*
 * FILES THAT SHOULD BE INCLUDED
GlacialElite
GlacialAspect
BottledChaos
Brainstalks
FrostRelic
IDP
SentientMeatHook
Wungus
GUP
THIS ONE
EngineerTurretSpawner
*/