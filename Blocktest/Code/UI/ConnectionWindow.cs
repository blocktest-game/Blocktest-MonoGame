using System.Net;
using Blocktest.Scenes;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
#pragma warning disable CS0618 // Type or member is obsolete, I don't care about this warning since it works fine

namespace Blocktest.UI;

public class ConnectionWindow : Window {
    public ConnectionWindow(BlocktestGame game) {
        Grid windowGrid = new() {
            RowSpacing = 8,
            ColumnSpacing = 8
        };

        Label label1 = new() {
            Text = "Enter IP:",
            Padding = new Thickness(5),
            GridColumn = 0,
            GridRow = 0
        };
        windowGrid.Widgets.Add(label1);

        TextBox ipBox = new() {
            Text = "127.0.0.1:9050",
            Padding = new Thickness(5),
            GridColumn = 1,
            GridRow = 0
        };
        windowGrid.Widgets.Add(ipBox);

        TextButton button = new() {
            Text = "Connect",
            Padding = new Thickness(5),
            GridColumn = 0,
            GridRow = 1,
            GridColumnSpan = 2,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        button.Click += (_, _) => {
            if (!IPEndPoint.TryParse(ipBox.Text, out IPEndPoint? ip)) {
                new DialogueWindow("Connect to server", "Invalid IP address.").ShowModal(Desktop);
                return;
            }
            game.SetScene(new GameScene(game, true, ip));
        };
        windowGrid.Widgets.Add(button);

        Title = "Connect to server";
        Content = windowGrid;
    }
}