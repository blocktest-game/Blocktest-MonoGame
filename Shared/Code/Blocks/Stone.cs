namespace Shared.Blocks
{
    public class Stone : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Stone";
            blockID = 3;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}