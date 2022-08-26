namespace RORMod.Items.Artifacts
{
    public class WeakAssKnees : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.frailty; set => RORMod.frailty = value; }
    }
}