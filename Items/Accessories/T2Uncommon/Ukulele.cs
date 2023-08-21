using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class Ukulele : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.GreenTier.Add((Type, () => Main.hardMode));
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1, silver: 50);
        }

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit)
        {
            if (Main.rand.NextBool(10) && victim.entity is NPC npc)
            {
                RORNPC.UkuleleLightning(npc, (int)(hit.Damage * 0.8), 0);

                if (Main.netMode != NetmodeID.Server)
                {
                    SoundEngine.PlaySound(RiskOfTerrain.GetSounds("ukulele/item_proc_chain_lightning_", 4).WithVolumeScale(0.4f), entity.entity.Center);
                }
            }
        }
    }
}