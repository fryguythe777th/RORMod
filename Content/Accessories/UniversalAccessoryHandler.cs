using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories
{
    public class UniversalAccessoryHandler : ILoadable
    {
        public static Dictionary<int, IAccessory> AccessoryLookup { get; private set; }

        private List<Item> items;
        public List<IAccessory> Accessories { get; private set; }

        public UniversalAccessoryHandler()
        {
            items = new List<Item>();
            Accessories = new List<IAccessory>();
        }

        void ILoadable.Load(Mod mod)
        {
            AccessoryLookup = new Dictionary<int, IAccessory>();
        }

        void ILoadable.Unload()
        {
            AccessoryLookup?.Clear();
            AccessoryLookup = null;
        }

        public void AddItemStack(Item item)
        {
            foreach (var i in items)
            {
                if (i.type == item.type)
                {
                    i.stack++;
                    return;
                }
            }
            items.Add(item.Clone());
            if (item.ModItem is IAccessory acc)
            {
                Accessories.Add(acc);
            }
            if (AccessoryLookup.TryGetValue(item.type, out var acc2))
            {
                Accessories.Add(acc2);
            }
        }

        public void ResetEffects(Entity entity)
        {
            foreach (var accessory in Accessories)
            {
                accessory.ResetEffects(new EntityInfo(entity));
            }
            items?.Clear();
            Accessories?.Clear();
        }

        public void ModifyHit(Entity entity, Entity victim, Entity projOrItem, ref int damage, ref float knockBack, ref bool crit)
        {
            foreach (var accessory in Accessories)
            {
                accessory.ModifyHit(new EntityInfo(entity), new EntityInfo(victim), projOrItem, ref damage, ref knockBack, ref crit);
            }
        }

        public void OnHit(Entity entity, Entity victim, Entity projOrItem, int damage, float knockBack, bool crit)
        {
            foreach (var accessory in Accessories)
            {
                accessory.OnHit(new EntityInfo(entity), new EntityInfo(victim), projOrItem, damage, knockBack, crit);
            }
        }

        public void ModifyHitBy(Entity entity, Entity attacker, ref int damage, ref float knockBack, ref bool crit)
        {
            foreach (var accessory in Accessories)
            {
                accessory.ModifyHitBy(new EntityInfo(entity), new EntityInfo(attacker), ref damage, ref knockBack, ref crit);
            }
        }

        public void OnHitBy(Entity entity, Entity attacker, int damage, float knockBack, bool crit)
        {
            foreach (var accessory in Accessories)
            {
                accessory.OnHitBy(new EntityInfo(entity), new EntityInfo(attacker), damage, knockBack, crit);
            }
        }

        public void OnKillEnemy(Entity entity, OnKillInfo info)
        {
            foreach (var accessory in Accessories)
            {
                accessory.OnKillEnemy(new EntityInfo(entity), info);
            }
        }

        public void ProcessTriggers(Player player, RORPlayer ror)
        {
            foreach (var accessory in Accessories)
            {
                accessory.ProcessTriggers(player, ror);
            }
        }

        public bool PreHurt(Player player, RORPlayer ror, bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            foreach (var accessory in Accessories)
            {
                if (!accessory.PreHurt(player, ror, pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter))
                    return false;
            }
            return true;
        }

        public bool PreKill(Player player, RORPlayer ror, double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            foreach (var accessory in Accessories)
            {
                if (!accessory.PreKill(player, ror, damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource))
                    return false;
            }
            return true;
        }

        public void Kill(Player player, RORPlayer ror, double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            foreach (var accessory in Accessories)
            {
                accessory.Kill(player, ror, damage, hitDirection, pvp, damageSource);
            }
        }

        public void Hurt(Player player, RORPlayer ror, bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            foreach (var accessory in Accessories)
            {
                accessory.Hurt(player, ror, pvp, quiet, damage, hitDirection, crit, cooldownCounter);
            }
        }

        public bool CanConsumeAmmo(Player player, RORPlayer ror)
        {
            foreach (var accessory in Accessories)
            {
                if (!accessory.CanConsumeAmmo(player, ror))
                    return false;
            }
            return true;
        }
    }
}