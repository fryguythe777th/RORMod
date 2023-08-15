using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Items.Accessories.Aspects;
using RiskOfTerrain.Buffs.WakeOfVultures;
using RiskOfTerrain.Projectiles.Elite;
using RiskOfTerrain.Content.OnHitEffects;
using RiskOfTerrain.Content.Elites;
using RiskOfTerrain.NPCs;
using RiskOfTerrain.Buffs;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class Brainstalks : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add((Type, () => Main.hardMode));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            NPC npc = Main.npc[info.whoAmI];

            for (int i = 0; i < RORNPC.RegisteredElites.Count; i++)
            {
                var l = new List<EliteNPCBase>(RORNPC.RegisteredElites);
                if (npc.GetGlobalNPC(l[i]).Active == true)
                {
                    entity.AddBuff(ModContent.BuffType<BrainstalksBuff>(), 600);
                }
            }
        }
    }
}
