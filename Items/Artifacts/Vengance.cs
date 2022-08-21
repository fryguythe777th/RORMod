namespace ROR2Artifacts.Items.Artifacts
{
    public class Vengance : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.VenganceActive; set => ROR2Artifacts.VenganceActive = value; }

        public override bool unimplemented => true;
    }
}