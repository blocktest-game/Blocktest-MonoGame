using Myra.Graphics2D.UI;
namespace Blocktest.UI;

public sealed class DialogueWindow : Window {
    public DialogueWindow(string title, string text) {
        Label label1 = new() {
            Text = text
        };


        Title = title;
        Content = label1;
    }
}