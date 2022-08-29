using RORMod.Content.Artifacts;

namespace RORMod.Items.Artifacts
{
    public class Honor : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.honor; set => ArtifactSystem.honor = value; }

        public override bool unimplemented => true;
    }
}