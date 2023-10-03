namespace Shared.Blocks
{
    public class Wood : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Wood";
            blockID = 4;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}