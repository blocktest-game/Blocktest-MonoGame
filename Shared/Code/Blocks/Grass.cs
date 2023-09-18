namespace Shared.Blocks
{
    public class Grass : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Grass";
            blockID = 2;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}