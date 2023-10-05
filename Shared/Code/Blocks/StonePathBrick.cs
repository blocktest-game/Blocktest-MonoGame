using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class StonePathBrick : BlockShared {
    public override void Initialize() {
        BlockName = "Stone Path Brick";
        BlockId = 14;
        BlockSmoothing = true;
        base.Initialize();
    }
}