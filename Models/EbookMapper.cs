using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using eBook_manager.Services;
using System.Collections.Generic;
using System.Linq;

namespace eBook_manager.Models
{
    public class EbookMapper
    {
        private readonly IAmazonDynamoDB _dynamoDBClient;

        public EbookMapper(IAmazonDynamoDB dynamoDBClient)
        {
            _dynamoDBClient = dynamoDBClient;
        }

        public EbookDTO MapDynamoDBItemToDTO(Dictionary<string, AttributeValue> dynamoDBItem)
        {
            return new EbookDTO
            {
                ISBN = dynamoDBItem["ISBN"].S,
                Authors = dynamoDBItem.ContainsKey("Authors") ? dynamoDBItem["Authors"].SS.ToList() : new List<string>(),
                Genre = dynamoDBItem["Genre"].S,
                Title = dynamoDBItem["Title"].S,
                Summary = dynamoDBItem["Summary"].S
            };
        }
    }
}
