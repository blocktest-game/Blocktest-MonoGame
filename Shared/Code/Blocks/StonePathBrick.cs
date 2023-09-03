namespace Shared.Blocks
{
    public class StonePathBrick : BlockShared
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