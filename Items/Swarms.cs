namespace ROR2Artifacts.Items
{
    public class Swarms : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.SwarmsActive; set => ROR2Artifacts.SwarmsActive = value; }

        public override bool unimplemented => true;
    }
}