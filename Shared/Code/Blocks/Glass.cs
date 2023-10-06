using Shared.Code.Block_System;
namespace Shared.Blocks
{
    public class Glass : BlockShared
    {
        public override void Initialize()
        {
            BlockName = "Glass";
            BlockId = 23;
            BlockSmoothing = false;
            base.Initialize();
        }
    }
}

