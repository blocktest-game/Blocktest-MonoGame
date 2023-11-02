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
            if (!IPEndPoint.TryParse(args[1], out IPEndPoint? ip)) {
                Console.WriteLine("Invalid IP address.");
                return;
            }
            using BlocktestGame game = new(ip);
            game.Run();
        } else {
            using BlocktestGame game = new();
            game.Run();
        }
    }
}