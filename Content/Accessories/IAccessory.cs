using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

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
        void OnHitBy(EntityInfo entity, EntityInfo attacker, Player.HurtInfo info);
        void OnUseItem(EntityInfo entity, Item item);
        void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref StatModifier damage, ref StatModifier knockBack, ref NPC.HitModifiers modifiers);
        void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit);
        void OnKillEnemy(EntityInfo entity, OnKillInfo info);

        void ProcessTriggers(Player player, RORPlayer ror);
        bool FreeDodge(Player player, Player.HurtInfo info);
        bool PreKill(Player player, RORPlayer ror, double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource);
        void Kill(Player player, RORPlayer ror, double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource);
        void Hurt(Player player, RORPlayer ror, Player.HurtInfo info);
        bool CanConsumeAmmo(Player player, RORPlayer ror);
    }
}