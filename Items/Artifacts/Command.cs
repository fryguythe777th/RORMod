using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Items.Artifacts
{
    public class Command : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.command; set => ArtifactSystem.command = value; }

        public override bool unimplemented => true;
    }
}