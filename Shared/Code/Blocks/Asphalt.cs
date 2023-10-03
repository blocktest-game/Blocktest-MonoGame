namespace Shared.Blocks
{
    public class Asphalt : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Asphalt";
            blockID = 10;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}