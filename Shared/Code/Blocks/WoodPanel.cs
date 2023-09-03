namespace Shared.Blocks
{
    public class WoodPanel : BlockShared
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