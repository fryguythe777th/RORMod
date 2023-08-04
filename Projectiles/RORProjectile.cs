using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Content.Elites;
using RiskOfTerrain.NPCs;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using RiskOfTerrain.Projectiles.Elite;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
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
        public bool hasCelestineProperties = false;
        public bool hasMalachiteProperties = false;
        public bool hasGhostlyProperties = false;

        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances => true;
        
        public static HashSet<int> CountsAsMissle { get; private set; }

        public override void Load()
        {
            CountsAsMissle = new HashSet<int>()
            {
                ProjectileID.ElectrosphereMissile,
                ProjectileID.RocketI,
                ProjectileID.ProximityMineI,
                ProjectileID.GrenadeI,
                ProjectileID.RocketII,
                ProjectileID.ProximityMineII,
                ProjectileID.GrenadeII,
                ProjectileID.RocketIII,
                ProjectileID.ProximityMineIII,
                ProjectileID.GrenadeIII,
                ProjectileID.RocketIV,
                ProjectileID.ProximityMineIV,
                ProjectileID.GrenadeIV,
                ProjectileID.RocketFireworkBlue,
                ProjectileID.RocketFireworkRed,
                ProjectileID.RocketFireworkGreen,
                ProjectileID.RocketFireworkYellow,
                ProjectileID.RocketSnowmanI,
                ProjectileID.RocketSnowmanII,
                ProjectileID.RocketSnowmanIII,
                ProjectileID.RocketSnowmanIV,
                ProjectileID.RocketFireworksBoxBlue,
                ProjectileID.RocketFireworksBoxRed,
                ProjectileID.RocketFireworksBoxGreen,
                ProjectileID.RocketFireworksBoxYellow,
                ProjectileID.VortexBeaterRocket,
                ProjectileID.Celeb2Rocket,
                ProjectileID.Celeb2RocketExplosive,
                ProjectileID.Celeb2RocketLarge,
                ProjectileID.Celeb2RocketExplosiveLarge,
                ProjectileID.ClusterRocketI,
                ProjectileID.ClusterMineI,
                ProjectileID.ClusterGrenadeI,
                ProjectileID.ClusterRocketII,
                ProjectileID.ClusterMineII,
                ProjectileID.ClusterGrenadeII,
                ProjectileID.WetRocket,
                ProjectileID.WetGrenade,
                ProjectileID.WetMine,
                ProjectileID.LavaRocket,
                ProjectileID.LavaGrenade,
                ProjectileID.LavaMine,
                ProjectileID.HoneyRocket,
                ProjectileID.HoneyGrenade,
                ProjectileID.HoneyMine,
                ProjectileID.MiniNukeRocketI,
                ProjectileID.MiniNukeMineI,
                ProjectileID.MiniNukeGrenadeI,
                ProjectileID.MiniNukeRocketII,
                ProjectileID.MiniNukeMineII,
                ProjectileID.MiniNukeGrenadeII,
                ProjectileID.DryRocket,
                ProjectileID.DryGrenade,
                ProjectileID.DryMine,
                ProjectileID.ClusterSnowmanRocketI,
                ProjectileID.ClusterSnowmanRocketII,
                ProjectileID.WetSnowmanRocket,
                ProjectileID.LavaSnowmanRocket,
                ProjectileID.HoneySnowmanRocket,
                ProjectileID.DrySnowmanRocket,
                ProjectileID.MiniNukeSnowmanRocketI,
                ProjectileID.MiniNukeSnowmanRocketII,
                ProjectileID.SantankMountRocket,
                ModContent.ProjectileType<AtGMissileProj>(),
                ModContent.ProjectileType<FireworksProj>(),
            };
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (projectile.friendly && !projectile.npcProj)
            {
                Main.player[projectile.owner].ROR().procRate = procRate;
            }

            if (spawnedFromElite)
            {
                modifiers.DisableCrit();
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
                    if (p.Prefix == Language.GetTextValue("Mods.RiskOfTerrain.CelestineElite"))
                    {
                        hasCelestineProperties = true;
                    }
                    if (p.Prefix == Language.GetTextValue("Mods.RiskOfTerrain.GhostElite"))
                    {
                        hasGhostlyProperties = true;
                        projectile.friendly = true;
                        projectile.hostile = false;
                        projectile.alpha = 100;
                    }
                    if (p.Prefix == Language.GetTextValue("Mods.RiskOfTerrain.MalachiteElite"))
                    {
                        hasMalachiteProperties = true;
                    }
                    if (p.Prefix == Language.GetTextValue("Mods.RiskOfTerrain.OverloadingElite"))
                    {
                        hasOverloaderProperties = true;
                    }
                }
            }

            if (projectile.friendly && CountsAsMissle.Contains(projectile.type) && Main.player[projectile.owner].ROR().accICBM)
            {
                projectile.damage = (int)(projectile.damage * 1.25f);
                if (projectile.ai[2] != 69)
                {
                    Projectile.NewProjectile(Main.player[projectile.owner].GetSource_FromThis(), projectile.Center, projectile.velocity.RotatedBy(0.5), projectile.type, projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1], 69);
                    Projectile.NewProjectile(Main.player[projectile.owner].GetSource_FromThis(), projectile.Center, projectile.velocity.RotatedBy(-0.5), projectile.type, projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1], 69); 
                }
            }
        }

        public override void AI(Projectile projectile)
        {
            if (hasGhostlyProperties)
            {
                projectile.friendly = true;
                projectile.hostile = false;
                projectile.alpha = 100;
            }
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            if (spawnedFromElite)
            {
                target.ROR().hitByBlazerProj = true;
            }

            if (hasCelestineProperties)
            {
                target.AddBuff(ModContent.BuffType<CelestineSlow>(), 180);
            }

            if (hasOverloaderProperties)
            {
                int p = Projectile.NewProjectile(Main.npc[projectile.owner].GetSource_FromThis(), target.Center + new Vector2(Main.rand.Next(0, 16), Main.rand.Next(0, 16)), Vector2.Zero, ModContent.ProjectileType<OverloadingBomb>(), 0, 0, Owner: projectile.owner, ai2: target.whoAmI, ai1: 1);
                Main.projectile[p].ROR().spawnedFromElite = true;
                Main.projectile[p].friendly = false;
                Main.projectile[p].hostile = true;
            }

            if (hasMalachiteProperties)
            {
                target.AddBuff(BuffID.Bleeding, 480);
            }
        }

        public ArmorShaderData ghostShader = GameShaders.Armor.GetShaderFromItemId(ItemID.FogboundDye);
        public static bool DrawingGhost;

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (hasGhostlyProperties)
            {
                if (ghostShader != null)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.Transform);
                    ghostShader.Apply(projectile);
                    DrawingGhost = true;
                }
            }
            return true;
        }

        public override void PostDraw(Projectile projectile, Color lightColor)
        {
            if (DrawingGhost)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.Transform);
                DrawingGhost = false;
            }
        }
    }
}