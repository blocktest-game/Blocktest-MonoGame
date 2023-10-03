namespace Shared.Blocks
{
    public class Brick : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Brick";
            blockID = 11;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}