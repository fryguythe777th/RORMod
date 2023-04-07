using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories
{
    public abstract class ModAccessory : ModItem, IAccessory
    {
        protected override bool CloneNewInstances => true;

        public int Stacks => Handler.GetItemStack(Type);
        int IAccessory.Type => Type;

        public UniversalAccessoryHandler Handler { get; set; }

        public virtual void OnEquip(EntityInfo entity)
        {
        }

        public virtual void OnUnequip(EntityInfo entity)
        {
        }

        public virtual void ResetEffects(EntityInfo entity)
        {
        }

        public virtual void PostUpdateEquips(EntityInfo entity)
        {
        }

        public virtual void PostUpdate(EntityInfo entity)
        {
        }

        public virtual void UpdateLifeRegeneration(EntityInfo entity)
        {
        }

        public virtual void ProcessTriggers(Player player, RORPlayer ror)
        {
        }

        public virtual void ModifyHitBy(EntityInfo entity, EntityInfo attacker, ref int damage, ref float knockBack, ref bool crit)
        {
        }

        public virtual void OnHitBy(EntityInfo entity, EntityInfo attacker, Player.HurtInfo info)
        {
        }

        public virtual void OnUseItem(EntityInfo entity, Item item)
        {
        }

        public virtual void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref StatModifier damage, ref StatModifier knockBack, ref NPC.HitModifiers modifiers)
        {
        }

        public virtual void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit)
        {
        }

        public virtual void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
        }

        public virtual bool FreeDodge(Player player, Player.HurtInfo info)
        {
            return true;
        }

        public virtual bool PreKill(Player player, RORPlayer ror, double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            return true;
        }

        public virtual void Kill(Player player, RORPlayer ror, double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
        }

        public virtual void Hurt(Player player, RORPlayer ror, Player.HurtInfo info)
        {
        }

        public virtual bool CanConsumeAmmo(Player player, RORPlayer ror)
        {
            return true;
        }
    }
}