using System;

namespace Blocktest
{
    /// <inheritdoc />
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using var game = new BlocktestGame();
            game.Run();
        }
    }
}
