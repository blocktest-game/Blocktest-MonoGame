using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class LargeStoneBrick : BlockShared {
    public override void Initialize() {
        BlockName = "Large Stone Brick";
        BlockId = 6;
        BlockSmoothing = true;
        base.Initialize();
    }
}