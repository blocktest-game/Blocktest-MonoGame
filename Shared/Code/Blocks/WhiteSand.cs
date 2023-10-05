using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class WhiteSand : BlockShared {
    public override void Initialize() {
        BlockName = "White Sand";
        BlockId = 16;
        BlockSmoothing = true;
        base.Initialize();
    }
}