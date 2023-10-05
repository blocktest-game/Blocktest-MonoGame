using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class Brick : BlockShared {
    public override void Initialize() {
        BlockName = "Brick";
        BlockId = 11;
        BlockSmoothing = true;
        base.Initialize();
    }
}