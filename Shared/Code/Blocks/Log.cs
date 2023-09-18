namespace Shared.Blocks
{
    public class Log : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Log";
            blockID = 7;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}