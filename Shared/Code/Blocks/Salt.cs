namespace Shared.Blocks
{
    public class Salt : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Salt";
            blockID = 20;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}