namespace Blocktest.Blocks
{
    public class Asphalt : Block
    {
        public override void Initialize()
        {
            blockName = "Asphalt";
            blockID = 9;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}