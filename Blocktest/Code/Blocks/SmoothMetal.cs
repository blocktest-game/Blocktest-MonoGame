namespace Blocktest.Blocks
{
    public class SmoothMetal : Block
    {
        public override void Initialize()
        {
            blockName = "Smooth Metal";
            blockID = 12;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}