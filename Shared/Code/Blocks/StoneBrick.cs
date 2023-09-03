namespace Shared.Blocks
{
    public class StoneBrick : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Stone Brick";
            blockID = 7;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}