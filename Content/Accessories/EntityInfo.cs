using Microsoft.Xna.Framework;
using RiskOfTerrain.Projectiles;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories
{
    public struct EntityInfo
    {
        public readonly Entity entity;

        public EntityInfo(Entity entity)
        {
            this.entity = entity;
        }

        public Vector2 Center => entity.Center;

        public int GetWeaponDamage(Item item)
        {
            if (entity is Player player)
            {
                return player.GetWeaponDamage(item);
            }
            return item.damage;
        }

        public Vector2 GetTargetPoint()
        {
            if (entity is NPC npc)
            {
                if (!npc.HasValidTarget)
                    return Vector2.Zero;
                if (npc.SupportsNPCTargets && npc.HasNPCTarget)
                {
                    return Main.npc[npc.TranslatedTargetIndex].Center;
                }
                else if (npc.HasPlayerTarget)
                {
                    return Main.player[npc.target].Center;
                }
            }
            else if (entity is Player player)
            {
                return Main.MouseWorld;
            }
            return Vector2.Zero;
        }

        public void GetProc(out float proc)
        {
            proc = 1f;
            if (entity is Player player)
            {
                proc = player.ROR().procRate;
            }
            proc *= 1f + RiskOfTerrain.globalProcRate;
        }

        public int RollLuck(int range)
        {
            if (entity is Player player)
            {
                if (player.ROR().accClover)
                {
                    int roll = player.RollLuck(range);
                    if (roll == 0)
                    {
                        return 0;
                    }
                }
                return player.RollLuck(range);
            }
            return Main.rand.Next(range);
        }

        public bool InDanger()
        {
            if (entity is Player player)
            {
                return player.ROR().InDanger;
            }
            return true;
        }

        public int IdleTime()
        {
            if (entity is Player player)
            {
                return player.ROR().idleTime;
            }
            return 0;
        }
        
        public void ClearBuff(int buffType)
        {
            if (entity is NPC npc)
            {
                int index = npc.FindBuffIndex(buffType);
                if (index != -1)
                    npc.DelBuff(index);
            }
            else if (entity is Player player)
            {
                player.ClearBuff(buffType);
            }
        }

        public bool GetBuffs(out int[] buffTypes, out int[] buffTimes, out int maxBuffs)
        {
            if (entity is Player player)
            {
                maxBuffs = Player.MaxBuffs;
                buffTypes = player.buffType;
                buffTimes = player.buffTime;
                return true;
            }
            if (entity is NPC npc)
            {
                maxBuffs = NPC.maxBuffs;
                buffTypes = npc.buffType;
                buffTimes = npc.buffTime;
                return true;
            }

            maxBuffs = 0;
            buffTypes = null;
            buffTimes = null;
            return false;
        }
        public void AddInfoBuff<T>() where T : ModBuff
        {
            if (entity is Player player)
            {
                if (player.HasBuff<T>() || player.CountBuffs() == Player.MaxBuffs)
                    return;
                for (int i = Player.MaxBuffs - 2; i >= 0; i--)
                {
                    player.buffType[i + 1] = player.buffType[i];
                    player.buffTime[i + 1] = player.buffTime[i];
                }
                player.buffType[0] = ModContent.BuffType<T>();
                player.buffTime[0] = 10;
                return;
            }
            AddBuff(ModContent.BuffType<T>(), 10, quiet: true);
        }
        public bool AddBuff(int type, int time, bool quiet = false)
        {
            if (entity is Player player)
            {
                player.AddBuff(type, time, quiet);
                return true;
            }
            if (entity is NPC npc)
            {
                npc.AddBuff(type, time, quiet);
                return true;
            }
            return false;
        }
        public bool HasBuff(int type)
        {
            if (entity is Player player)
            {
                return player.HasBuff(type);
            }
            if (entity is NPC npc)
            {
                return npc.HasBuff(type);
            }
            return false;
        }

        public int OwnedProjectilesCount(int type)
        {
            if (entity is Player player)
            {
                return player.ownedProjectileCounts[type];
            }
            return OwnedProjectilesCountLong(type);
        }

        public int OwnedProjectilesCountLong(int type)
        {
            int count = 0;
            if (entity is Player player)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == type && OwnsThisProjectile(Main.projectile[i]))
                    {
                        count++;
                    }
                }
            }
            if (entity is NPC npc)
            {
                //for (int i = 0; i < Main.maxProjectiles; i++)
                //{
                //    if (Main.projectile[i].active && OwnsThisProjectile(Main.projectile[i]))
                //    {
                //
                //    }
                //}
            }
            return count;
        }

        public bool OwnsThisProjectile(Projectile projectile)
        {
            if (entity is Player player)
            {
                return projectile.owner == player.whoAmI && ! projectile.hostile;
            }
            return false;
        }

        public void AddLifeRegen(int regen)
        {
            if (entity is Player player)
            {
                if (regen < 0)
                {
                    player.lifeRegen = Math.Min(player.lifeRegen, 0);
                }
                player.lifeRegen += regen;
            }
            else if (entity is NPC npc)
            {
                if (regen < 0)
                {
                    npc.lifeRegen = Math.Min(npc.lifeRegen, 0);
                }
                npc.lifeRegen += regen; // will this even work?
            }
        }

        public bool IsMe()
        {
            if (entity is Player player)
            {
                return player.whoAmI == Main.myPlayer;
            }
            if (entity is Projectile proj)
            {
                return proj.owner == Main.myPlayer;
            }
            return Main.netMode != NetmodeID.MultiplayerClient;
        }

        public int GetProjectileOwnerID()
        {
            if (entity is Player player)
            {
                return player.whoAmI;
            }
            if (entity is Projectile proj)
            {
                return proj.owner;
            }
            return Main.myPlayer;
        }

        public IEntitySource? GetSource_Accessory(Item item)
        {
            if (entity is Player player)
            {
                return player.GetSource_Accessory(item);
            }
            else
            {
                return null;
            }
        }
    }
}