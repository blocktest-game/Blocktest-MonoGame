namespace Shared.Blocks
{
    public class Dirt : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Dirt";
            blockID = 1;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}