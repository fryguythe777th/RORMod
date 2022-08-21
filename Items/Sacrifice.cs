namespace ROR2Artifacts.Items
{
    public class Sacrifice : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.SacrificeActive; set => ROR2Artifacts.SacrificeActive = value; }

        public override bool unimplemented => true;
    }
}