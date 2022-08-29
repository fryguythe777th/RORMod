using RORMod.Content.Artifacts;

namespace RORMod.Items.Artifacts
{
    public class Metamorphosis : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.metamorphosis; set => ArtifactSystem.metamorphosis = value; }

        public override bool unimplemented => true;
    }
}