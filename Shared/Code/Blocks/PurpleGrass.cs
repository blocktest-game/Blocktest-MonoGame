using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class PurpleGrass : BlockShared {
    public override void Initialize() {
        BlockName = "Purple Grass";
        BlockId = 17;
        BlockSmoothing = true;
        base.Initialize();
    }
}