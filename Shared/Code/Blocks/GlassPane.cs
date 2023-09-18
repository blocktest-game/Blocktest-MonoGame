namespace Shared.Blocks
{
    public class GlassPane : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Glass Pane";
            blockID = 5;
            blockSmoothing = true;
            smoothSelf = true;
            base.Initialize();
        }
    }
}