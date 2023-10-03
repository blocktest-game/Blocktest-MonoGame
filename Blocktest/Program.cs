using Blocktest;

public class Program
{
    static public void Main(String[] args)
    {
        int argLength = args.Length;
        if(argLength == 2)
        {
            String argument = args[0];              // This can be cleaned up, may want to use a config object
            String value = args[1];
            if(argument.Equals("connect"))
            {
                using BlocktestGame game = new(value);
                game.Run();
            }
        }
        else
        {
            using BlocktestGame game = new();
            game.Run();
        }
    }
}
