namespace ROR2Artifacts.Items
{
    public class Glass : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.GlassActive; set => ROR2Artifacts.GlassActive = value; }
    }
}