namespace Shared.Blocks
{
    public class Pucestone : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Pucestone";
            blockID = 19;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}