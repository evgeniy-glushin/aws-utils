using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;

namespace S3Utils
{
    class Program
    {
        private const string bucketName = "yevhenii.ua.dev";

        //private const string bucketName = "*** versioning-enabled bucket name ***";
        //// Specify your bucket region (an example region is shown).
        //private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;
        static string key = "AKIAWXXRZRULY3GO2OG7";
        static string secret = "fgyiVk5cf0qQ8q5uuBsulHFxwi9gQAVdYxcUipEM";

        //private static IAmazonS3 s3Client;

        static void Main(string[] args)
        {
            var client = new AmazonS3Client(key, secret, new AmazonS3Config() { RegionEndpoint = RegionEndpoint.EUNorth1 });

            //Delete(client);

            // Retrieve an existing configuration. 
            var lifeCycleConfiguration = RetrieveLifecycleConfigAsync(client);

            //Console.WriteLine(JsonConvert);

            // Add a new rule.
            lifeCycleConfiguration.Rules.Add(new LifecycleRule
            {
                Id = "Expiration Rule",
                Filter = new LifecycleFilter()
                {
                    LifecycleFilterPredicate = new LifecyclePrefixPredicate()
                    {
                        Prefix = "nested forlder/"
                    }
                },
                Expiration = new LifecycleRuleExpiration()
                {
                    Days = 1
                }
            });

            AddExampleLifecycleConfigAsync(client, lifeCycleConfiguration);

            var newConfig = RetrieveLifecycleConfigAsync(client);

            Console.ReadKey();
        }

        static void AddExampleLifecycleConfigAsync(IAmazonS3 client, LifecycleConfiguration configuration)
        {

            PutLifecycleConfigurationRequest request = new PutLifecycleConfigurationRequest
            {
                BucketName = bucketName,
                
                Configuration = configuration
            };
            var response = client.PutLifecycleConfigurationAsync(request).Result;
        }

        static LifecycleConfiguration RetrieveLifecycleConfigAsync(IAmazonS3 client)
        {
            GetLifecycleConfigurationRequest request = new GetLifecycleConfigurationRequest
            {
                BucketName = bucketName,                
            };
            var response = client.GetLifecycleConfigurationAsync(request).Result;
            var configuration = response.Configuration;
            return configuration;
        }

        private static void Delete(AmazonS3Client client)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = "test-file.txt"
            };

            Console.WriteLine("Deleting an object");
            var res = client.DeleteObjectAsync(deleteObjectRequest).Result;
            Console.WriteLine("the object deleted");
        }
    }
}
