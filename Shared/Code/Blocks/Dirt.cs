namespace Shared.Blocks
{
    public class Dirt : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Dirt";
            blockID = 0;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}