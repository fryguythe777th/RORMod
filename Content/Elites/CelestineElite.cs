using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Items.Accessories.Aspects;
using RiskOfTerrain.NPCs;
using RiskOfTerrain.Projectiles.Elite;
using Terraria;
using Terraria.Audio;
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

        public static int numCelestinesIngame = 0;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void AI(NPC npc)
        {
            if (active)
            {
                npc.ROR().npcSpeedStat *= 0.25f;

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
                    if (Main.npc[i].active && !Main.npc[i].friendly)
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
            projectile.timeLeft = 2;
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (active)
            {
                target.AddBuff(ModContent.BuffType<CelestineSlow>(), 180);
            }
        }

        public override bool CanRoll(NPC npc)
        {
            if (Main.hardMode)
            {
                return !ServerConfig.Instance.CelestineElitesDisable;
            }
            return false;
        }

        public override void OnBecomeElite(NPC npc)
        {
            npc.lifeMax = (int)(npc.lifeMax * 4f);
            npc.life = (int)(npc.life * 4f);
            npc.npcSlots *= 8f;
            npc.value *= 4;
        }

        public override void OnKill(NPC npc)
        {
            if (active)
            {
                int rollNumber = npc.boss ? 1000 : 4000;
                if (Main.player[Player.FindClosest(npc.Center, 500, 500)].RollLuck(rollNumber) == 0)
                {
                    int i = Item.NewItem(npc.GetSource_GiftOrReward(), npc.Center, ModContent.ItemType<CelestineAspect>());
                    Main.item[i].velocity = new Vector2(0, -4);
                }
            }
        }
    }
}