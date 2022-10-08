using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Items.Artifacts
{
    public class Spite : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.spite; set => ArtifactSystem.spite = value; }
    }
}