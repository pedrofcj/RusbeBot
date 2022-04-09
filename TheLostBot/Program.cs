using System.Threading.Tasks;

namespace TheLostBot;

internal class Program
{
    public static Task Main(string[] args)
        => Startup.RunAsync(args);
}