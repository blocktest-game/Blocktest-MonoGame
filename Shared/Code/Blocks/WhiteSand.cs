namespace Shared.Blocks
{
    public class WhiteSand : BlockShared
    {
        public override void Initialize()
        {
            blockName = "White Sand";
            blockID = 15;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}