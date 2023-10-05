using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class PucestoneSlab : BlockShared {
    public override void Initialize() {
        BlockName = "Pucestone Slab";
        BlockId = 22;
        BlockSmoothing = true;
        base.Initialize();
    }
}