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

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class WakeOfVultures : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.HasBuff(ModContent.BuffType<BlazingWOV>()))
            {
                BlazingAspect.BlazingUpdate(player);
            }

            if (player.HasBuff(ModContent.BuffType<CelestineWOV>()))
            {
                CelestineAspect.CelestineUpdate(player);
            }

            if (player.HasBuff(ModContent.BuffType<MendingWOV>()))
            {
                MendingAspect.MendingUpdate(player);
            }

            if (player.HasBuff(ModContent.BuffType<OverloadingWOV>()))
            {
                OverloadingAspect.OverloadingUpdate(player);
            }
        }

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit)
        {
            if (entity.HasBuff(ModContent.BuffType<BlazingWOV>()))
            {
                BlazingAspect.BlazingOnHit(victim, projOrItem);
            }

            if (entity.HasBuff(ModContent.BuffType<CelestineWOV>()))
            {
                CelestineAspect.CelestineOnHit(victim);
            }

            if (entity.HasBuff(ModContent.BuffType<GlacialWOV>()))
            {
                GlacialAspect.GlacialOnHit(victim);
            }

            if (entity.HasBuff(ModContent.BuffType<OverloadingWOV>()))
            {
                OverloadingAspect.OverloadingOnHit(entity, victim, projOrItem);
            }
        }

        public override void PostUpdate(EntityInfo entity)
        {
            if (entity.HasBuff(ModContent.BuffType<CelestineWOV>()))
            {
                CelestineAspect.CelestinePostUpdate(entity, Item);
            }
        }

        public override void Hurt(Player player, RORPlayer ror, Player.HurtInfo info)
        {
            if (player.HasBuff(ModContent.BuffType<GlacialWOV>()))
            {
                GlacialAspect.GlacialHurt(player, info);
            }

            if (player.HasBuff(ModContent.BuffType<MendingWOV>()))
            {
                MendingAspect.MendingHurt(player, info);
            }
        }

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            NPC npc = Main.npc[info.whoAmI];

            for (int i = 0; i < RORNPC.RegisteredElites.Count; i++)
            {
                var l = new List<EliteNPCBase>(RORNPC.RegisteredElites);
                if (npc.GetGlobalNPC(l[i]).Active == true)
                {
                    switch (i)
                    {
                        case 0:
                            entity.AddBuff(ModContent.BuffType<BlazingWOV>(), 900);
                            break;

                        case 1:
                            entity.AddBuff(ModContent.BuffType<CelestineWOV>(), 900);
                            break;

                        case 2:
                            entity.AddBuff(ModContent.BuffType<GlacialWOV>(), 900);
                            break;

                        case 3:
                            //entity.AddBuff(ModContent.BuffType<MalachiteWOV>(), 900);
                            break;

                        case 4:
                            entity.AddBuff(ModContent.BuffType<MendingWOV>(), 900);
                            break;

                        case 5:
                            entity.AddBuff(ModContent.BuffType<OverloadingWOV>(), 900);
                            break;

                        case 6:
                            //entity.AddBuff(ModContent.BuffType<PerfectedWOV>(), 900);
                            break;

                        case 7:
                            //entity.AddBuff(ModContent.BuffType<VoidtouchedWOV>(), 900);
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}