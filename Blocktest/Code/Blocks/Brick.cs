namespace Blocktest.Blocks
{
    public class Brick : Block
    {
        public override void Initialize()
        {
            blockName = "Brick";
            blockID = 10;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}