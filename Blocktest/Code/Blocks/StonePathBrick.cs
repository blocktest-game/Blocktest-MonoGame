namespace Blocktest.Blocks
{
    public class StonePathBrick : Block
    {
        public override void Initialize()
        {
            blockName = "Stone Path Brick";
            blockID = 13;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}