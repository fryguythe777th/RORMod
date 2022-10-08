using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Items.Artifacts
{
    public class Enigma : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.enigma; set => ArtifactSystem.enigma = value; }
    }
}