using RORMod.Content.Artifacts;

namespace RORMod.Items.Artifacts
{
    public class Enigma : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.enigma; set => ArtifactSystem.enigma = value; }
    }
}