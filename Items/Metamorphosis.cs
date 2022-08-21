namespace ROR2Artifacts.Items
{
    public class Metamorphosis : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.MetamorphosisActive; set => ROR2Artifacts.MetamorphosisActive = value; }

        public override bool unimplemented => true;
    }
}