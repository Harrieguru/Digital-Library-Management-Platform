using Amazon.DynamoDBv2;
using eBook_manager.Repositories.Interfaces;
using eBook_manager.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json"); // Load appsettings.json

var awsRegion = builder.Configuration["AWSCredentials:Region"]; // AWS region
var accessKey = builder.Configuration["AWSCredentials:AccessKey"]; // AWS access key ID
var secretKey = builder.Configuration["AWSCredentials:SecretKey"]; // AWS secret access key

var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "eBook_manager", Version = "v1" });
});

builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    var clientConfig = new AmazonDynamoDBConfig
    {
        RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsRegion)
    };

    return new AmazonDynamoDBClient(credentials, clientConfig);
});

// Register repositories
builder.Services.AddScoped<IEbookRepository, DynamoDbEbookRepository>();
builder.Services.AddScoped<IUserRepository, DynamoDbUserRepository>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "eBook_manager V1");
    });
}
else
{
    app.Use(async (context, next) =>
    {
        //if the request is not HTTPS and the ECS service only supports HTTP
        if (context.Request.Scheme != "https")
        {
            // Skip redirection and proceed to the next middleware
            await next();
        }
        else
        {
            // Redirect HTTPS requests to HTTP 
            var httpUrl = "http://" + context.Request.Host + context.Request.Path;
            context.Response.Redirect(httpUrl, permanent: true);
        }
    });
}


app.UseAuthorization();
app.MapControllers();

app.Run();
