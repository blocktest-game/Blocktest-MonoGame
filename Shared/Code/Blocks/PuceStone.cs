using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class Pucestone : BlockShared {
    public override void Initialize() {
        BlockName = "Pucestone";
        BlockId = 19;
        BlockSmoothing = true;
        base.Initialize();
    }
}