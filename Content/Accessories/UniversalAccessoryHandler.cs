using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories
{
    public class UniversalAccessoryHandler : ILoadable
    {
        public static Dictionary<int, IAccessory> AccessoryLookup { get; private set; }

        public readonly Dictionary<int, int> Stacks;
        public readonly Dictionary<int, Item> MostRecentItemReference;
        public readonly List<IAccessory> EquippedLastFrame;
        public readonly List<IAccessory> Accessories;

        public UniversalAccessoryHandler()
        {
            Stacks = new Dictionary<int, int>();
            MostRecentItemReference = new Dictionary<int, Item>();
            EquippedLastFrame = new List<IAccessory>();
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

        public int GetItemStack(int itemID)
        {
            if (Stacks.TryGetValue(itemID, out var stack))
                return stack;
            return 0;
        }

        public Item GetItemReference(int itemID)
        {
            if (MostRecentItemReference.TryGetValue(itemID, out var stack))
                return stack;
            return null;
        }

        public void AddItemStack(Item item)
        {
            MostRecentItemReference[item.type] = item;
            
            int baseStack = 0;
            foreach (var type in Stacks.Keys)
            {
                if (type == item.type)
                {
                    baseStack = Stacks[item.type];
                }
            }

            Stacks[item.type] = baseStack + item.stack;

            if (baseStack > 0)
                return;

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
            var info = new EntityInfo(entity);
            foreach (var accessory in Accessories)
            {
                accessory.Handler = this;
                accessory.ResetEffects(info);
                if (EquippedLastFrame.Find((a) => a.Type == accessory.Type) == null)
                {
                    accessory.OnEquip(info);
                }
            }
            foreach (var accessory in EquippedLastFrame)
            {
                if (Accessories.Find((a) => a.Type == accessory.Type) == null)
                {
                    accessory.OnUnequip(info);
                    accessory.Handler = null;
                }
            }
            EquippedLastFrame.Clear();
            EquippedLastFrame.AddRange(Accessories);
            Stacks?.Clear();
            MostRecentItemReference?.Clear();
            Accessories?.Clear();
        }

        public void PostUpdateEquips(Entity entity)
        {
            foreach (var accessory in Accessories)
            {
                accessory.PostUpdateEquips(new EntityInfo(entity));
            }
        }

        public void PostUpdate(Entity entity)
        {
            foreach (var accessory in Accessories)
            {
                accessory.PostUpdate(new EntityInfo(entity));
            }
        }

        public void UpdateLifeRegeneration(Entity entity)
        {
            foreach (var accessory in Accessories)
            {
                accessory.UpdateLifeRegeneration(new EntityInfo(entity));
            }
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