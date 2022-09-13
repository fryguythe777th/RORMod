using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Projectiles
{
    internal class RORProjectile : GlobalProjectile
    {
        public override bool PreAI(Projectile projectile)
        {
            return true;
        }
    }
}
