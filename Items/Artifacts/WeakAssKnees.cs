using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Items.Artifacts
{
    public class WeakAssKnees : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.frailty; set => ArtifactSystem.frailty = value; }
    }
}