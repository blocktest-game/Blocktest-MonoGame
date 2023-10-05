using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class Grass : BlockShared {
    public override void Initialize() {
        BlockName = "Grass";
        BlockId = 2;
        BlockSmoothing = true;
        base.Initialize();
    }
}