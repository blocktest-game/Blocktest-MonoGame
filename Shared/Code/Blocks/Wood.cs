using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class Wood : BlockShared {
    public override void Initialize() {
        BlockName = "Wood";
        BlockId = 4;
        BlockSmoothing = true;
        base.Initialize();
    }
}