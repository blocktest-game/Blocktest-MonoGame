namespace Blocktest.Blocks
{
    public class Dirt : Block
    {
        public override void Initialize()
        {
            blockName = "Dirts";
            blockID = 0;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}