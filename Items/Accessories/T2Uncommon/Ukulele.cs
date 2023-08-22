using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.NPCs;
using RiskOfTerrain.Projectiles.Misc;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

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


        public static void UkuleleLightning(NPC npc, int damage, int timesProcced, bool skipDamage = false)
        {
            npc.ROR().hasBeenStruckByUkuleleLightning = true;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && RORNPC.Distance(npc, Main.npc[i]) <= 150 && Main.npc[i].ROR().hasBeenStruckByUkuleleLightning == false && !Main.npc[i].friendly && Main.npc[i].lifeMax > 5 && Main.npc[i].damage > 0)
                {
                    UkuleleLightning(Main.npc[i], damage, timesProcced + 1);

                    for (int j = 0; j < RORNPC.Distance(npc, Main.npc[i]); j++)
                    {
                        Projectile.NewProjectile(npc.GetSource_FromThis(), npc.position, Vector2.Zero, ModContent.ProjectileType<LightningEffectProj>(), 0, 0, ai0: i, Owner: npc.whoAmI);
                    }
                }
            }

            if (!skipDamage)
            {
                NPC.HitInfo hit = new NPC.HitInfo
                {
                    DamageType = DamageClass.Default,
                    SourceDamage = damage,
                    Damage = damage,
                    Crit = false,
                    Knockback = 0f,
                    HitDirection = 0
                };
                npc.StrikeNPC(hit);
                NetMessage.SendStrikeNPC(npc, hit);
            }
        }

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit)
        {
            if (Main.rand.NextBool(10) && victim.entity is NPC npc)
            {
                UkuleleLightning(npc, (int)(hit.Damage * 0.8), 0, true);

                if (Main.netMode != NetmodeID.Server)
                {
                    SoundEngine.PlaySound(RiskOfTerrain.GetSounds("ukulele/item_proc_chain_lightning_", 4).WithVolumeScale(0.4f), entity.entity.Center);
                }
            }
        }
    }
}