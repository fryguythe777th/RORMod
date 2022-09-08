using RORMod.NPCs;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RORMod.Content.Elites
{
    public abstract class EliteNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances => true;

        public virtual string Prefix { get => Language.GetTextValue($"Mods.{Mod.Name}.{Name}"); }

        public virtual ArmorShaderData Shader { get; }

        protected bool active;
        public virtual bool Active { get => active; set => active = value; }

        public override void SetStaticDefaults()
        {
            RORNPC.RegisteredElites.Add(this);
        }
    }
}