using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using eBook_manager.Models;
using eBook_manager.Repositories.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Amazon;


namespace eBook_manager.Repositories
{
    public class DynamoDbEbookRepository : IEbookRepository
    {
        private readonly IAmazonDynamoDB _dynamoDBClient;

        public DynamoDbEbookRepository(IConfiguration configuration)
        {
            var accessKey = configuration["AWSCredentials:AccessKey"];
            var secretKey = configuration["AWSCredentials:SecretKey"];
            var region = RegionEndpoint.GetBySystemName(configuration["AWSCredentials:Region"]);

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var config = new AmazonDynamoDBConfig { RegionEndpoint = region };

            _dynamoDBClient = new AmazonDynamoDBClient(credentials, config);
        }

        public async Task<EbookDTO> GetEbookByISBN(string isbn)
        {
            var table = Table.LoadTable(_dynamoDBClient, "eBooks");
            var document = await table.GetItemAsync(isbn);

            if (document != null)
            {
                var ebook = new EbookDTO
                {
                    ISBN = document["ISBN"].AsString(),
                    Authors = document["Authors"].AsListOfString(),
                    Genre = document["Genre"].AsString(),
                    Title = document["Title"].AsString(),
                    Summary = document["Summary"].AsString()
                };

                return ebook;
            }

            return null; // Handles the case where the ebook with the given ISBN is not found
        }

        public async Task AddEbook(EbookDTO ebook)
        {
            var table = Table.LoadTable(_dynamoDBClient, "eBooks");
            var document = new Document();
            document["ISBN"] = ebook.ISBN;
            document["Authors"] = ebook.Authors;
            document["Genre"] = ebook.Genre;
            document["Title"] = ebook.Title;
            document["Summary"] = ebook.Summary;

            await table.PutItemAsync(document);
        }

        public async Task<IEnumerable<EbookDTO>> GetAllEbooks()
        {
            var table = Table.LoadTable(_dynamoDBClient, "eBooks");
            var scanFilter = new ScanFilter();
            var search = table.Scan(scanFilter);
            var documents = await search.GetNextSetAsync();

            var ebooks = documents.Select(document => new EbookDTO
            {
                ISBN = document["ISBN"].AsString(),
                Authors = document["Authors"].AsListOfString(),
                Genre = document["Genre"].AsString(),
                Title = document["Title"].AsString(),
                Summary = document["Summary"].AsString()
            
            });

            return ebooks;
        }

        public async Task UpdateEbook(EbookDTO ebook)
        {
            var table = Table.LoadTable(_dynamoDBClient, "eBooks");
            var document = await table.GetItemAsync(ebook.ISBN);

            if (document != null)
            {
                document["Authors"] = ebook.Authors;
                document["Genre"] = ebook.Genre;
                document["Title"] = ebook.Title;
                document["Summary"] = ebook.Summary;

                await table.UpdateItemAsync(document);
            }
        }

        public async Task PartialUpdateEbook(string isbn, JsonPatchDocument<EbookDTO> patchDocument)
        {
            var ebook = await GetEbookByISBN(isbn);

            if (ebook == null)
            {
                // Handle if the ebook is not found
                return;
            }

            // Apply patch document to the ebook
            patchDocument.ApplyTo(ebook);

            // Update the ebook after applying patch
            await UpdateEbook(ebook);
        }


        //retrieve books by genre
        public async Task<IEnumerable<EbookDTO>> GetEbooksByGenre(string genre)
        {
            var table = Table.LoadTable(_dynamoDBClient, "eBooks");
            var query = new QueryOperationConfig
            {
                IndexName = "getByGenre", // index name
                Filter = new QueryFilter("Genre", QueryOperator.Equal, genre)
            };

            var search = table.Query(query);
            var documents = await search.GetNextSetAsync();

            var ebooks = documents.Select(document => new EbookDTO
            {
                ISBN = document["ISBN"].AsString(),
                Authors = document["Authors"].AsListOfString(),
                Genre = document["Genre"].AsString(),
                Title = document["Title"].AsString(),
                Summary = document["Summary"].AsString()
            });

            return ebooks;
        }




        public async Task DeleteEbook(string isbn)
        {
            var table = Table.LoadTable(_dynamoDBClient, "eBooks");
            await table.DeleteItemAsync(isbn);
        }
    }
}
