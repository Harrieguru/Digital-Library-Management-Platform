using eBook_manager.Repositories.Interfaces;
using Amazon.DynamoDBv2.DocumentModel;
using eBook_manager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using BCrypt.Net;


namespace eBook_manager.Repositories
{
    public class DynamoDbUserRepository : IUserRepository
    {
        private readonly IAmazonDynamoDB _dynamoDBClient;
        private const string TableName = "Users";

        public DynamoDbUserRepository(IAmazonDynamoDB dynamoDBClient)
        {
            _dynamoDBClient = dynamoDBClient;
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            var table = Table.LoadTable(_dynamoDBClient, TableName);
            var document = await table.GetItemAsync(email);

            if (document != null)
            {
                var user = new UserDTO
                {
                    username = document["username"].AsString(),
                    email = document["email"].AsString(),
                    password = document["password"].AsString()
                };

                return user;
            }

            return null; // Handles the case where the user with the given email is not found
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            var table = Table.LoadTable(_dynamoDBClient, TableName);
            var scanFilter = new ScanFilter();
            var search = table.Scan(scanFilter);
            var documents = await search.GetNextSetAsync();

            var users = documents.Select(document => new UserDTO
            {
                username = document["username"].AsString(),
                email = document["email"].AsString(),
                password = document["password"].AsString()
            });

            return users;
        }

        public async Task AddUser(UserDTO user)
        {
            var table = Table.LoadTable(_dynamoDBClient, TableName);
            var document = new Document();
            document["username"] = user.username;
            document["email"] = user.email;
            document["password"] = BCrypt.Net.BCrypt.HashPassword(user.password);

            await table.PutItemAsync(document);
        }

        public async Task UpdateUser(string email,UserDTO user)
        {
            var table = Table.LoadTable(_dynamoDBClient, TableName);
            var document = await table.GetItemAsync(user.email);

            if (document != null)
            {
                document["username"] = user.username;
                document["password"] = BCrypt.Net.BCrypt.HashPassword(user.password);

                await table.UpdateItemAsync(document);
            }
        }



        public async Task DeleteUserByEmail(string email)
        {
            var table = Table.LoadTable(_dynamoDBClient, TableName);
            await table.DeleteItemAsync(email);
        }
    }
}
