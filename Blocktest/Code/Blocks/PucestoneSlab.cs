namespace Blocktest.Blocks
{
    public class PucestoneSlab : Block
    {
        public override void Initialize()
        {
            blockName = "Pucestone Slab";
            blockID = 22;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}