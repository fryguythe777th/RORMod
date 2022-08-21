namespace ROR2Artifacts.Items
{
    public class Vengance : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.VenganceActive; set => ROR2Artifacts.VenganceActive = value; }

        public override bool unimplemented => true;
    }
}