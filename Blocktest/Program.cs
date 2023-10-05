using System.Net;
namespace Blocktest;

public abstract class Program {
    public static void Main(string[] args) {
        int argLength = args.Length;
        if (argLength == 2) {
            if (!args[0].EndsWith("connect")) {
                Console.WriteLine("Invalid argument.");
                return;
            }
            if (!IPAddress.TryParse(args[1], out IPAddress? _)) {
                Console.WriteLine("Invalid IP address.");
                return;
            }
            using BlocktestGame game = new(args[1]);
            game.Run();
        } else {
            using BlocktestGame game = new();
            game.Run();
        }
    }
}