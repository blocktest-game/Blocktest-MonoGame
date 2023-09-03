namespace Shared.Blocks
{
    public class Log : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Log";
            blockID = 6;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}