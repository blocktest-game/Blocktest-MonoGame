namespace Blocktest.Blocks
{
    public class Salt : Block
    {
        public override void Initialize()
        {
            blockName = "Salt";
            blockID = 20;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}