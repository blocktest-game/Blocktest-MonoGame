using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class Dirt : BlockShared {
    public override void Initialize() {
        BlockName = "Dirt";
        BlockId = 1;
        BlockSmoothing = true;
        base.Initialize();
    }
}