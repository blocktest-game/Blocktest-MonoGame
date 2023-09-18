namespace Shared.Blocks
{
    public class Concrete : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Concrete";
            blockID = 15;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}