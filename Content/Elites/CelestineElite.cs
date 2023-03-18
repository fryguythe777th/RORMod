using Microsoft.Xna.Framework;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Projectiles.Misc;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Elites
{
    public class CelestineElite : EliteNPCBase
    {
        public override ArmorShaderData Shader => GameShaders.Armor.GetShaderFromItemId(ItemID.FogboundDye);

        public bool hasSpawnedBubble = false;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void AI(NPC npc)
        {
            if (active)
            {
                if (!hasSpawnedBubble)
                {
                    Projectile i = Projectile.NewProjectileDirect(npc.GetSource_FromThis(), npc.Center, Vector2.Zero,
                    ModContent.ProjectileType<CelestineProj>(), 0, 0f, ai0: npc.whoAmI);
                    i.ai[0] = npc.whoAmI;
                    UpdateProjectile(npc, i);
                    hasSpawnedBubble = true;
                }

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    //somebody doesn't have elite prefixes (probably check if main.npc[i].active
                    if (Main.npc[i].active)
                    {
                        bool canBeInvised = true;
                        Main.npc[i].GetElitePrefixes(out var prefixes);
                        foreach (var p in prefixes)
                        {
                            if (p.Prefix == Language.GetTextValue("Mods.RiskOfTerrain.CelestineElite"))
                            {
                                canBeInvised = false;
                            }
                        }
                        if (npc.Distance(Main.npc[i].Hitbox.ClosestDistance(npc.Center)) < 240f && canBeInvised)
                        {
                            Main.npc[i].AddBuff(ModContent.BuffType<CelestineInvis>(), 2);
                        }
                    }
                }

                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<CelestineProj>() && npc.whoAmI == Main.projectile[i].ai[0])
                    {
                        UpdateProjectile(npc, Main.projectile[i]);
                        return;
                    }
                }
            }
        }

        public void UpdateProjectile(NPC npc, Projectile projectile)
        {
            projectile.scale = MathHelper.Lerp(projectile.scale, 480f, 0.2f);
            projectile.Center = npc.Center;
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit)
        {
            if (active)
            {
                target.AddBuff(ModContent.BuffType<CelestineSlow>(), 180);
            }
        }

        public override bool CanRoll(NPC npc)
        {
            return false;
        }

        public override void OnBecomeElite(NPC npc)
        {
            npc.lifeMax = (int)(npc.lifeMax * 4f);
            npc.life = (int)(npc.life * 4f);
            npc.npcSlots *= 8f;
            npc.value *= 4;
        }
    }
}