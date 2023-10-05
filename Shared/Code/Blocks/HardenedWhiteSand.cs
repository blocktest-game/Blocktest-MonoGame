using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class HardenedWhiteSand : BlockShared {
    public override void Initialize() {
        BlockName = "Hardened White Sand";
        BlockId = 18;
        BlockSmoothing = true;
        base.Initialize();
    }
}