using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class WhiteSandstoneBrick : BlockShared {
    public override void Initialize() {
        BlockName = "White Sandstone Brick";
        BlockId = 21;
        BlockSmoothing = true;
        base.Initialize();
    }
}