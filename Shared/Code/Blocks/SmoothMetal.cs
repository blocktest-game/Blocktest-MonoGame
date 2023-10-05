using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class SmoothMetal : BlockShared {
    public override void Initialize() {
        BlockName = "Smooth Metal";
        BlockId = 13;
        BlockSmoothing = true;
        base.Initialize();
    }
}