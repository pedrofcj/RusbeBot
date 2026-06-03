using Microsoft.Extensions.Configuration;
using RusbeBot.Data.Values;

namespace RusbeBot.Core.Helpers;

public class AwsHelper
{
    public static void Start(IConfigurationRoot configuration)
    {
        AwsCredentials.SetCredentials(configuration["aws:accessKeyId"], configuration["aws:secretAccessKey"]);
    }
}