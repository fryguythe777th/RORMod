namespace ROR2Artifacts.Items.Artifacts
{
    public class Soul : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.SoulActive; set => ROR2Artifacts.SoulActive = value; }
    }
}