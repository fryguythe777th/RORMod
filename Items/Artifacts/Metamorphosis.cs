namespace ROR2Artifacts.Items.Artifacts
{
    public class Metamorphosis : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.MetamorphosisActive; set => ROR2Artifacts.MetamorphosisActive = value; }

        public override bool unimplemented => true;
    }
}