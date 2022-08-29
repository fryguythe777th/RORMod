using RORMod.Content.Artifacts;

namespace RORMod.Items.Artifacts
{
    public class Spite : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.spite; set => ArtifactSystem.spite = value; }
    }
}