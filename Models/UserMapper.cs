using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace eBook_manager.Models
{
    public class UserMapper
    {
        private readonly IAmazonDynamoDB _dynamoDBClient;

        public UserMapper(IAmazonDynamoDB dynamoDBClient)
        {
            _dynamoDBClient = dynamoDBClient;
        }

        public UserDTO MapDynamoDBItemToDTO(Dictionary<string, AttributeValue> dynamoDBitem)
        {
            return new UserDTO
            {
                username = dynamoDBitem["username"].S,
                email = dynamoDBitem["email"].S,
                password = dynamoDBitem["password"].S
            };
        }
    }
}
