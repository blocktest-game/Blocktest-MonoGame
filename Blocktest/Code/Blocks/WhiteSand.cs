namespace Blocktest.Blocks
{
    public class WhiteSand : Block
    {
        public override void Initialize()
        {
            blockName = "White Sand";
            blockID = 16;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}