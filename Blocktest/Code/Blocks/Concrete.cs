namespace Blocktest.Blocks
{
    public class Concrete : Block
    {
        public override void Initialize()
        {
            blockName = "Concrete";
            blockID = 14;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}