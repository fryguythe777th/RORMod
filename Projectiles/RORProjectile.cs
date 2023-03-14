using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Content.Elites;
using RiskOfTerrain.NPCs;
using RiskOfTerrain.Projectiles.Misc;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles
{
    public class RORProjectile : GlobalProjectile
    {
        public float procRate = 1f;

        public bool spawnedFromElite = false;
        public bool hasOverloaderProperties = false;

        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances => true;

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.friendly && !projectile.npcProj)
            {
                Main.player[projectile.owner].ROR().procRate = procRate;
            }

            if (spawnedFromElite)
            {
                crit = false;
            }
        }

        public override void SetDefaults(Projectile projectile)
        {
            if (spawnedFromElite)
            {
                projectile.knockBack = 0;
            }
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (Helpers.HereditarySource(source, out var parent) && parent is NPC parentNPC)
            {
                parentNPC.GetElitePrefixes(out var prefixes);
                foreach (var p in prefixes)
                {
                    if (p.Prefix == Language.GetTextValue("Mods.RiskOfTerrain.OverloadingElite"))
                    {
                        hasOverloaderProperties = true;
                    }
                }
            }
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref int damage, ref bool crit)
        {
            if (spawnedFromElite)
            {
                crit = false;
                target.ROR().hitByBlazerProj = true;
            }

            if (hasOverloaderProperties)
            {
                Main.NewText("bombed");
                int p = Projectile.NewProjectile(Main.npc[projectile.owner].GetSource_FromThis(), target.Center + new Vector2(Main.rand.Next(0, 16), Main.rand.Next(0, 16)), Vector2.Zero, ModContent.ProjectileType<OverloadingBomb>(), 0, 0, ai0: target.whoAmI, ai1: 1);
                Main.projectile[p].ROR().spawnedFromElite = true;
                Main.projectile[p].friendly = false;
                Main.projectile[p].hostile = true;
                Main.projectile[p].ai[1] = 1;
            }
        }
    }
}