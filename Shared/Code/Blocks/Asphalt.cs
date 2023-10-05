using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class Asphalt : BlockShared {
    public override void Initialize() {
        BlockName = "Asphalt";
        BlockId = 10;
        BlockSmoothing = true;
        base.Initialize();
    }
}