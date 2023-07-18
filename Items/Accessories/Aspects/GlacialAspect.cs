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
    public class GlacialAspect : GenericAspect
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Cyan;
        }

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit)
        {
            GlacialOnHit(victim);
        }

        public static void GlacialOnHit(EntityInfo victim)
        {
            if (victim.entity is Player player)
            {
                player.AddBuff(ModContent.BuffType<GlacialSlow>(), 300);
            }

            if (victim.entity is NPC npc)
            {
                npc.AddBuff(ModContent.BuffType<GlacialSlow>(), 300);
            }
        }

        public override void Hurt(Player player, RORPlayer ror, Player.HurtInfo info)
        {
            GlacialHurt(player, info);
        }

        public static void GlacialHurt(Player player, Player.HurtInfo info)
        {
            if (info.Damage > player.statLifeMax * 0.15)
            {
                Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<GlacialBomb>(), 0, 0, Owner: player.whoAmI, ai0: 1);
            }
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight(Item.Center, new Vector3(1f, 0.95f, 0.85f));
        }
    }
}