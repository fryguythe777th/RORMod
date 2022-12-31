using System.Collections.Generic;
using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class OldGuillotine : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
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

        public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref int damage, ref float knockBack, ref bool crit)
        {
            NPC npc = Main.npc[victim.entity.whoAmI];
            npc.GetElitePrefixes(out List<Content.Elites.EliteNPCBase> prefixes);

            if (prefixes.Count > 0 && npc.life <= npc.lifeMax * 0.13)
            {
                npc.life = 1;
                npc.StrikeNPC(npc.lifeMax, 0f, 0);
                npc.active = false;
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, npc.lifeMax);
                }
            }
        }
    }
}