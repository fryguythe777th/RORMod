using RiskOfTerrain.Projectiles;
using System;
using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Content.Accessories
{
    public struct EntityInfo
    {
        public readonly Entity entity;

        public EntityInfo(Entity entity)
        {
            this.entity = entity;
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
            return false;
        }

        public int IdleTime()
        {
            if (entity is Player player)
            {
                return player.ROR().idleTime;
            }
            return 0;
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

        public bool CanSpawnProjectileOnThisClient()
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
    }
}