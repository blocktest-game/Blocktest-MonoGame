namespace Shared.Blocks
{
    public class LargeStoneBrick : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Large Stone Brick";
            blockID = 5;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}