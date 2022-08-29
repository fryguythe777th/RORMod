using RORMod.Content.Artifacts;

namespace RORMod.Items.Artifacts
{
    public class Command : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.command; set => ArtifactSystem.command = value; }

        public override bool unimplemented => true;
    }
}