namespace Blocktest.Blocks
{
    public class LargeStoneBrick : Block
    {
        public override void Initialize()
        {
            blockName = "Large Stone Brick";
            blockID = 15;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}