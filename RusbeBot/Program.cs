using System.Threading.Tasks;

namespace RusbeBot;

internal class Program
{
    public static Task Main(string[] args)
        => Startup.RunAsync(args);
}