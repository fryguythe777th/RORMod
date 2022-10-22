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
            proc *= (1f + RiskOfTerrain.globalProcRate);
        }

        public int RollLuck(int range)
        {
            if (entity is Player player)
            {
                return player.RollLuck(range);
            }
            return Main.rand.Next(range);
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

        public int SpawnOwner()
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