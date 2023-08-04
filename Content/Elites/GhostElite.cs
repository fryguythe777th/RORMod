using System;
using Microsoft.Xna.Framework;
using RiskOfTerrain.Items.Accessories.Aspects;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Elites
{
    public class GhostElite : EliteNPCBase
    {
        public override ArmorShaderData Shader => GameShaders.Armor.GetShaderFromItemId(ItemID.FogboundDye);
        public int owner = -1;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public int damageTimer = 60;

        public override void AI(NPC npc)
        {
            if (active)
            {
                npc.friendly = true;
                npc.alpha = 100;

                if (damageTimer == 0)
                {
                    NPC.HitInfo hit = new NPC.HitInfo();
                    hit.Crit = false;
                    hit.HitDirection = 0;
                    hit.Knockback = 0f;
                    hit.Damage = (int)Math.Round(npc.lifeMax / 60f);
                    hit.DamageType = DamageClass.Default;
                    npc.StrikeNPC(hit);
                    damageTimer = 60;
                }
                else
                {
                    damageTimer--;
                }

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (npc.whoAmI != i && !Main.npc[i].friendly && Main.npc[i].lifeMax > 5 && Main.npc[i].damage > 0 && npc.Hitbox.Intersects(Main.npc[i].Hitbox) && Main.npc[i].immune[owner] == 0 && !Main.npc[i].GetGlobalNPC<GhostElite>().Active)
                    {
                        NPC.HitInfo meleeHit = new NPC.HitInfo();

                        if (owner > -1)
                        {
                            meleeHit.Crit = Main.rand.Next(1, 101) / 100 <= (int)Main.player[owner].GetTotalCritChance<GenericDamageClass>();
                            meleeHit.HitDirection = Math.Sign(npc.Center.X - Main.npc[i].Center.X);
                            meleeHit.Knockback = (int)Main.player[owner].GetTotalKnockback<GenericDamageClass>().ApplyTo(1f);
                            meleeHit.Damage = (int)Main.player[owner].GetTotalDamage<GenericDamageClass>().ApplyTo(npc.damage);
                            meleeHit.DamageType = DamageClass.Default;
                        }
                        else
                        {
                            meleeHit.Crit = Main.rand.NextBool(40);
                            meleeHit.HitDirection = Math.Sign(npc.Center.X - Main.npc[i].Center.X);
                            meleeHit.Knockback = 1f;
                            meleeHit.Damage = npc.damage;
                            meleeHit.DamageType = DamageClass.Default;
                        }

                        Main.npc[i].StrikeNPC(meleeHit);
                        Main.npc[i].immune[owner] = 10;
                    }
                }
            }
        }

        public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
        {
            if (active)
            {
                if (owner > -1)
                {
                    return player.team != Main.player[owner].team || item.type == ItemID.Flymeal;
                }
                else
                {
                    return false;
                }
            }
            return null;
        }

        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            if (active)
            {
                return projectile.hostile || projectile.type == ProjectileID.RottenEgg;
            }
            return null;
        }

        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (active)
            {
                if (owner > -1)
                {
                    return target.team != Main.player[owner].team;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public override bool CanHitNPC(NPC npc, NPC target)
        {
            if (active)
            {
                return !target.friendly;
            }
            return true;
        }

        public override bool CanRoll(NPC npc)
        {
            return false;
        }

        public override void OnBecomeElite(NPC npc)
        {
            npc.lifeMax = (int)(npc.lifeMax * 1f);
            npc.life = (int)(npc.life * 1f);
            npc.npcSlots *= 0f;
            npc.value *= 0;
        }
    }
}