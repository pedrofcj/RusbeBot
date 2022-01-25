using Amazon.Runtime;

namespace Data.Values
{
    public class AwsCredentials
    {
        private static string _accessKeyId;
        private static string _secretAccessKey;

        private static BasicAWSCredentials _basicAwsCredentials;
        public static BasicAWSCredentials BasicAwsCredentials => _basicAwsCredentials ??= new BasicAWSCredentials(_accessKeyId, _secretAccessKey);

        public static void SetCredentials(string accessKeyId, string secretKeyId)
        {
            _accessKeyId = accessKeyId;
            _secretAccessKey = secretKeyId;
        }

    }
}