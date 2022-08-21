namespace ROR2Artifacts.Items.Artifacts
{
    public class Honor : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.HonorActive; set => ROR2Artifacts.HonorActive = value; }

        public override bool unimplemented => true;
    }
}