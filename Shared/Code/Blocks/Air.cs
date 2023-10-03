namespace Shared.Blocks
{
    public class Air : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Air";
            blockID = 0;
            blockSmoothing = false;
            base.Initialize();
        }
    }
}