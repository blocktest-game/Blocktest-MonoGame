using Myra.Graphics2D.UI;

namespace Blocktest.UI;

public sealed class DialogueWindow : Window {
    public DialogueWindow(string title, string text) {
        var label1 = new Label {
            Text = text
        };


        Title = title;
        Content = label1;
    }
}