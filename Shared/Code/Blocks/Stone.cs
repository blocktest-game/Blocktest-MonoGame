using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class Stone : BlockShared {
    public override void Initialize() {
        BlockName = "Stone";
        BlockId = 3;
        BlockSmoothing = true;
        base.Initialize();
    }
}