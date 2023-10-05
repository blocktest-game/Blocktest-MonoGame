using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class Salt : BlockShared {
    public override void Initialize() {
        BlockName = "Salt";
        BlockId = 20;
        BlockSmoothing = true;
        base.Initialize();
    }
}