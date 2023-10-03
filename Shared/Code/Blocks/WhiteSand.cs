namespace Shared.Blocks
{
    public class WhiteSand : BlockShared
    {
        public override void Initialize()
        {
            blockName = "White Sand";
            blockID = 16;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}