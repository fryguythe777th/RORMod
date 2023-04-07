using System.Collections.Generic;
using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class OldGuillotine : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.GreenTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1, silver: 50);
        }

        public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref StatModifier damage, ref StatModifier knockBack, ref NPC.HitModifiers modifiers)
        {
            NPC npc = Main.npc[victim.entity.whoAmI];
            npc.GetElitePrefixes(out List<Content.Elites.EliteNPCBase> prefixes);

            if (prefixes.Count > 0 && npc.life <= npc.lifeMax * 0.13)
            {
                modifiers.SetInstantKill();
            }
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
    }
}