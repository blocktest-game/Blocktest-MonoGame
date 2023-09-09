namespace Blocktest.Blocks
{
    public class PurpleGrass : Block
    {
        public override void Initialize()
        {
            blockName = "Purple Grass";
            blockID = 17;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}