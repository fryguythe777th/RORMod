using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Items.Artifacts
{
    public class Death : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.death; set => ArtifactSystem.death = value; }
    }
}