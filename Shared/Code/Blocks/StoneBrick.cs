namespace Shared.Blocks
{
    public class StoneBrick : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Stone Brick";
            blockID = 8;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}