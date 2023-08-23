namespace Blocktest.Blocks
{
    public class WoodPanel : Block
    {
        public override void Initialize()
        {
            blockName = "Wood Panel";
            blockID = 11;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}