namespace Shared.Blocks
{
    public class Wood : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Wood";
            blockID = 3;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}