namespace Shared.Blocks
{
    public class Asphalt : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Asphalt";
            blockID = 9;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}