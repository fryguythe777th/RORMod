using RORMod.Content.Artifacts;

namespace RORMod.Items.Artifacts
{
    public class Dissonance : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.dissonance; set => ArtifactSystem.dissonance = value; }
    }
}