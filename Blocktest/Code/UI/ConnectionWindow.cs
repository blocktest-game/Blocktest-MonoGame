using Blocktest.Scenes;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
#pragma warning disable CS0618 // Type or member is obsolete, I don't care about this warning since it works fine

namespace Blocktest.UI; 

public class ConnectionWindow : Window {
    public ConnectionWindow(BlocktestGame game) {
        var windowGrid = new Grid {
            RowSpacing = 8,
            ColumnSpacing = 8
        };
        
        var label1 = new Label {
            Text = "Enter IP:",
            Padding = new Thickness(5),
            GridColumn = 0,
            GridRow = 0
        };
        windowGrid.Widgets.Add(label1);
        
        var textBox = new TextBox {
            Text = "127.0.0.1",
            Padding = new Thickness(5),
            GridColumn = 1,
            GridRow = 0
        };
        windowGrid.Widgets.Add(textBox);
        
        var button = new TextButton {
            Text = "Connect",
            Padding = new Thickness(5),
            GridColumn = 0,
            GridRow = 1,
            GridColumnSpan = 2,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        
        button.Click += (_, _) => {
            game.SetScene(new GameScene(game, true, textBox.Text));
        };
        windowGrid.Widgets.Add(button);
        
        Content = windowGrid;
    }
}