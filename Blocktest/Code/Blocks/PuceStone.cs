namespace Blocktest.Blocks
{
    public class Pucestone : Block
    {
        public override void Initialize()
        {
            blockName = "Pucestone";
            blockID = 19;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}