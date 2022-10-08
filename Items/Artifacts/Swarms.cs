using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Items.Artifacts
{
    public class Swarms : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.swarms; set => ArtifactSystem.swarms = value; }

        public override bool unimplemented => true;
    }
}