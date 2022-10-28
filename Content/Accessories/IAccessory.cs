using Terraria;
using Terraria.DataStructures;

namespace RiskOfTerrain.Content.Accessories
{
    public interface IAccessory
    {
        UniversalAccessoryHandler Handler { get; internal set; }
        int Stacks { get; }
        int Type { get; }

        void OnEquip(EntityInfo entity);
        void OnUnequip(EntityInfo entity);
        void ResetEffects(EntityInfo entity);
        void PostUpdateEquips(EntityInfo entity);
        void PostUpdate(EntityInfo entity);
        void UpdateLifeRegeneration(EntityInfo entity);
        void ModifyHitBy(EntityInfo entity, EntityInfo attacker, ref int damage, ref float knockBack, ref bool crit);
        void OnHitBy(EntityInfo entity, EntityInfo attacker, int damage, float knockBack, bool crit);
        void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref int damage, ref float knockBack, ref bool crit);
        void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, int damage, float knockBack, bool crit);
        void OnKillEnemy(EntityInfo entity, OnKillInfo info);

        void ProcessTriggers(Player player, RORPlayer ror);
        bool PreHurt(Player player, RORPlayer ror, bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter);
        bool PreKill(Player player, RORPlayer ror, double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource);
        void Kill(Player player, RORPlayer ror, double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource);
        void Hurt(Player player, RORPlayer ror, bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter);
        bool CanConsumeAmmo(Player player, RORPlayer ror);
    }
}