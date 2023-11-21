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

/*
 * CHANGELOGS
- Added Bottled Chaos
- Added Brainstalks
- Added Interstellar Desk Plant
- Added the Frost Relic
- Added Sentient Meat Hook
- Added Glacial Elites
- Added Her Biting Embrace
- Added Weeping Fungus
- Added the Gup to singleplayer worlds
- War Horn now activates for a much shorter duration the first time you hit an enemy
- Hopoo Feather now grants an additional jump
- Soldier's Syringe no longer has a damage penalty
- Shattering Justice's damage increase has been halved
- Added a crafting recipe for Delicate Watch
- Added a crafting recipe for Topaz Brooch
- Added a crafting recipe for Wax Quail
- Monster Tooth Necklace can now drop from Dripplers and Blood Zombies
- Medkit can now drop from the Nurse NPC
- Warbanner can now drop from the Dark Mage boss
- Hopoo Feather can now drop from Harpies
- Crowbar can now drop from Goblin Thieves
- Lens-Maker's Glasses can now drop from Demon Eyes
- Harvester's Scythe can now drop from Wraiths
- Death Mark can now drop from the Rune Wizard
- Wake of Vultures can now drop from Vultures
- Symbiotic Scorpion can now drop from Sand Poachers
- Frost Relic can now drop from Ice Slimes
- Sentient Meat Hook can now drop from the Brain of Cthulhu
- Bottled Chaos can now drop from Goblin Summoners
- Brainstalks can now drop from the Corrupt Mimic
- Berzerker's Pauldron can now drop from the Crimson Mimic
- The Arms Dealer will now sell Armor Piercing Round and Backup Magazine
- The Party Girl will now sell Bundle of Fireworks
- The Zoologist will now sell Red Whip
- The Dryad will now sell Leeching Seed
- The Demolitionist will now sell Sticky Explosives and Brilliant Behemoth
- The Cyborg will now sell ICBM
- The Merchant will now sell Roll of Pennies
- Old Guillotine and Old War Stealthkit can generate in wooden surface chests
- Reloading Shurikens can generate in shadow chests
- Aegis can generate in marble chests
- Shattering Justice can generate in granite chests
- Fixed multiplayer bugs related to Overloading Elites and Ukulele 
*/
