/* Glass crashes game right now
namespace Shared.Blocks
{
    public class Glass : BlockShared
    {
        public override void Initialize()
        {
            blockName = "Glass";
            blockID = -1;       // Needs to be changed if this is fixed
            blockSmoothing = false;
            base.Initialize();
        }
    }
}
*/