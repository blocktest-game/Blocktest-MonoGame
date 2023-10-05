using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class WoodPanel : BlockShared {
    public override void Initialize() {
        BlockName = "Wood Panel";
        BlockId = 12;
        BlockSmoothing = true;
        base.Initialize();
    }
}