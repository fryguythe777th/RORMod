using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Items.Artifacts
{
    public class Glass : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.glass; set => ArtifactSystem.glass = value; }
    }
}