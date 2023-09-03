namespace Shared.Blocks
{
    public class Grass : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Grass";
            blockID = 1;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}