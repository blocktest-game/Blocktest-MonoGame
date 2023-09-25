namespace Shared.Blocks
{
    public class PucestoneSlab : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Pucestone Slab";
            blockID = 22;
            blockSmoothing = true;
            base.Initialize();
        }
    }
}