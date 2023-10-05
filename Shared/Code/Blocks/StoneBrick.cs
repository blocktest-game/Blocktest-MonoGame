using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class StoneBrick : BlockShared {
    public override void Initialize() {
        BlockName = "Stone Brick";
        BlockId = 8;
        BlockSmoothing = true;
        base.Initialize();
    }
}