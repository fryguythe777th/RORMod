namespace ROR2Artifacts.Items.Artifacts
{
    public class WeakAssKnees : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.FrailtyActive; set => ROR2Artifacts.FrailtyActive = value; }
    }
}