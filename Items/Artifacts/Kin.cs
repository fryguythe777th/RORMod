namespace ROR2Artifacts.Items.Artifacts
{
    public class Kin : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.KinActive; set => ROR2Artifacts.KinActive = value; }

        public override bool unimplemented => true;
    }
}