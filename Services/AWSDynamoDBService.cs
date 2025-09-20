using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;

namespace eBook_manager.Services
{
    public class AWSDynamoDBService
    {
        private readonly AmazonDynamoDBClient _dynamoDBClient;

        public AWSDynamoDBService(IConfiguration configuration)
        {
            var accessKey = configuration["AWSCredentials:AccessKey"];
            var secretKey = configuration["AWSCredentials:SecretKey"];
            var region = RegionEndpoint.GetBySystemName(configuration["AWSCredentials:Region"]);

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var config = new AmazonDynamoDBConfig { RegionEndpoint = region };

            _dynamoDBClient = new AmazonDynamoDBClient(credentials, config);
        }

        public AmazonDynamoDBClient GetDynamoDBClient()
        {
            return _dynamoDBClient;
        }
    }
}
