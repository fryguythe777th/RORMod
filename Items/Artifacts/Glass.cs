using RORMod.Content.Artifacts;

namespace RORMod.Items.Artifacts
{
    public class Glass : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.glass; set => ArtifactSystem.glass = value; }
    }
}