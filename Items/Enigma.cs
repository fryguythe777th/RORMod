namespace ROR2Artifacts.Items
{
    public class Enigma : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.EnigmaActive; set => ROR2Artifacts.EnigmaActive = value; }
    }
}