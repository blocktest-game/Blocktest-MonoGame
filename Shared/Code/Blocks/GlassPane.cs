using Shared.Code.Block_System;
namespace Shared.Code.Blocks;

public class GlassPane : BlockShared {
    public override void Initialize() {
        BlockName = "Glass Pane";
        BlockId = 5;
        BlockSmoothing = true;
        SmoothSelf = true;
        base.Initialize();
    }
}