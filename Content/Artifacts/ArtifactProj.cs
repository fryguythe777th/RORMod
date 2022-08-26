using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace RORMod.Content.Artifacts
{
    public class ArtifactProj : GlobalProjectile
    {
        public int npcOwnerType;

        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_OnHit onHit)
            {
                OwnerCheck(onHit.EntityStruck, projectile);
            }
            else if (source is EntitySource_Parent parent)
            {
                OwnerCheck(parent.Entity, projectile);
            }
            else if (source is EntitySource_HitEffect hitEffect)
            {
                OwnerCheck(hitEffect.Entity, projectile);
            }
            else if (source is EntitySource_Death death)
            {
                OwnerCheck(death.Entity, projectile);
            }
        }

        public void OwnerCheck(Entity ent, Projectile proj)
        {
            if (ent is NPC npc)
            {
                npcOwnerType = npc.netID;
            }
        }

        public override bool? CanHitNPC(Projectile projectile, NPC target)
        {
            if (RORMod.chaos)
            {
                if (npcOwnerType == 0 || (target.type != npcOwnerType &&
                    (!ArtifactNPC.Chaos_HitBlacklist.TryGetValue(npcOwnerType, out var l) || !l.Contains(target.whoAmI))))
                {
                    return true;
                }
            }
            return null;
        }
    }
}