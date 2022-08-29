using RORMod.Content.Artifacts;

namespace RORMod.Items.Artifacts
{
    public class Sacrifice : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.sacrifice; set => ArtifactSystem.sacrifice = value; }

        public override bool unimplemented => true;
    }
}