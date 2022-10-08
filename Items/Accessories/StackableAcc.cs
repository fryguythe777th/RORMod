using RiskOfTerrain.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories
{
    public abstract class StackableAcc : ModItem
    {
        // No need to sync this info since it will just be used as temporary refreshed acc data
        public int stack;

        protected override bool CloneNewInstances => true;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            SetStack(player, stack + 1);
        }

        public virtual void Reset()
        {
            stack = 0;
        }
        public virtual void ResetEffects(Player player, RORPlayer ror)
        {
            Reset();
        }

        public virtual void CalculateStats(Player player, RORPlayer ror)
        {
        }
        public void CalculateStats(Player player)
        {
            CalculateStats(player, player.ROR());
        }

        public virtual void SetStack(Player player, RORPlayer ror, int stack)
        {
            this.stack = stack;
            CalculateStats(player, ror);
        }
        public void SetStack(Player player, int stack)
        {
            SetStack(player, player.ROR(), stack);
        }

        public virtual void CalculateStats(NPC npc)
        {
        }
        public virtual void ResetEffects(NPC npc, RORNPC ror)
        {
            Reset();
        }
    }
}