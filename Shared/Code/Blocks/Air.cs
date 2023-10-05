using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class Air : BlockShared {
    public override void Initialize() {
        BlockName = "Air";
        BlockId = 0;
        BlockSmoothing = false;
        base.Initialize();
    }
}