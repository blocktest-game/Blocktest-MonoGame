namespace Shared.Blocks
{
    public class SmoothMetal : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Smooth Metal";
            blockID = 13;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}