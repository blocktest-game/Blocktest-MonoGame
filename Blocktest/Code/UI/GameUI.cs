using Blocktest.Block_System;
using Blocktest.Scenes;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using Shared.Code.Block_System;

namespace Blocktest.UI; 

public sealed class GameUI : Grid {
    public ComboBox BlockSelector;
    
    public GameUI(GameScene scene) {
        BlockSelector = new ComboBox {
            GridColumn = 0,
            GridRow = 0,
            Width = 200,
            Height = 40,
            Padding = new Thickness(5)
        };
        
        foreach (var block in BlockManagerShared.AllBlocks) {
            var blockItem = new ListItem {
                Text = block.Value.BlockName,
                Image = BlockSpritesManager.AllBlocksSprites[block.Key].BlockSprite,
                Id = block.Key
            };
            BlockSelector.Items.Add(blockItem);
        }
        BlockSelector.SelectedIndexChanged += (_, _) => {
            scene.BlockSelected = BlockSelector.SelectedIndex ?? 0;
        };
        BlockSelector.SelectedIndex = scene.BlockSelected;
        
        Widgets.Add(BlockSelector);
        
        var buildMode = new CheckBox {
            GridColumn = 1,
            GridRow = 0,
            Text = "Build Mode",
            Width = 200,
            Height = 40,
            Padding = new Thickness(5),
            IsChecked = scene.BuildMode
        };
        buildMode.TouchDown += (_, _) => {
            scene.SetBuildMode(!buildMode.IsChecked);
        };
        
        Widgets.Add(buildMode);
    }
}