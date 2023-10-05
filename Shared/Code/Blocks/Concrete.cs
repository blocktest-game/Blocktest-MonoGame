using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class Concrete : BlockShared {
    public override void Initialize() {
        BlockName = "Concrete";
        BlockId = 15;
        BlockSmoothing = true;
        base.Initialize();
    }
}