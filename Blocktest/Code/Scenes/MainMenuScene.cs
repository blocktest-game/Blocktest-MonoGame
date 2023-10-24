using Blocktest.UI;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;

namespace Blocktest.Scenes; 


public class MainMenuScene : IScene {
    private Desktop _desktop;
    private VerticalStackPanel _mainMenu;
    
    public MainMenuScene(BlocktestGame game) {
        _mainMenu = new VerticalStackPanel {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            BorderThickness = new Thickness(1),
            Background = new SolidBrush("#404040FF"),
            Border = new SolidBrush("#1BA1E2FF")
        };
        
        var titleLabel = new Label {
            Text = "Blocktest",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Padding = new Thickness(8)
        };
        _mainMenu.Widgets.Add(titleLabel);
        
        var newGameButton = new TextButton { 
            Text = "New Game",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Padding = new Thickness(5) 
        };
        newGameButton.Click += (_, _) => {
            game.SetScene(new GameScene(game, false, null));
        };
        _mainMenu.Widgets.Add(newGameButton);
        
        var connectButton = new TextButton { 
            Text = "Connect",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Padding = new Thickness(5)
        };
        connectButton.Click += (_, _) => {
            new ConnectionWindow(game).ShowModal(_desktop);
        };
        _mainMenu.Widgets.Add(connectButton);
        
        var exitButton = new TextButton { 
            Text = "Exit",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Padding = new Thickness(5)
        };
        exitButton.Click += (_, _) => {
            game.Exit();
        };
        _mainMenu.Widgets.Add(exitButton);
        
        _desktop = new Desktop { Root = _mainMenu };
    }
    
    public void Update(GameTime gameTime) { }

    public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice) {
        graphicsDevice.Clear(Color.CadetBlue);
        
        _desktop.Render();
    }

    public void EndScene() { }
}