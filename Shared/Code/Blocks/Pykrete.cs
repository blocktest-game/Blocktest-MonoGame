using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class Pykrete : BlockShared {
    public override void Initialize() {
        BlockName = "Pykrete";
        BlockId = 9;
        BlockSmoothing = true;
        base.Initialize();
    }
}