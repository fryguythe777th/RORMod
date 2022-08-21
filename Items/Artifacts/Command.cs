namespace ROR2Artifacts.Items.Artifacts
{
    public class Command : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.CommandActive; set => ROR2Artifacts.CommandActive = value; }

        public override bool unimplemented => true;
    }
}