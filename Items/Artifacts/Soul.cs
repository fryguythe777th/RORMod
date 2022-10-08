using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Items.Artifacts
{
    public class Soul : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.soul; set => ArtifactSystem.soul = value; }
    }
}