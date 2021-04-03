using System;

namespace Blocktest
{
    /// <inheritdoc />
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new BlocktestGame();
            game.Run();
        }
    }
}
