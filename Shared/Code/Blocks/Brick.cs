namespace Shared.Blocks
{
    public class Brick : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Brick";
            blockID = 10;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}