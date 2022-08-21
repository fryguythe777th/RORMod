namespace ROR2Artifacts.Items.Artifacts
{
    public class Death : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.DeathActive; set => ROR2Artifacts.DeathActive = value; }
    }
}