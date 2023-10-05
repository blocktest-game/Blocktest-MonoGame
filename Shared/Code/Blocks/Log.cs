using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class Log : BlockShared {
    public override void Initialize() {
        BlockName = "Log";
        BlockId = 7;
        BlockSmoothing = true;
        base.Initialize();
    }
}