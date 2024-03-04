using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using DynamoDB.Sample.API.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var clientDynamo = ConfigDynamoDb(builder);
CreateTableIfNotExists(clientDynamo).Wait();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("api/user", async (IDynamoDBContext context, User user) =>
{
    await context.SaveAsync(user);
    return Results.Created($"/api/user/{user.Id}", user);
});

app.MapGet("api/user", async (IDynamoDBContext context) =>
{
    var users = await context.ScanAsync<User>(new List<ScanCondition>()).GetRemainingAsync();
    return Results.Ok(users);
});

app.MapGet("api/user/{id}", async (IDynamoDBContext context, string id) =>
{
    var user = await context.LoadAsync<User>(id);
    return Results.Ok(user);
});

app.MapPut("api/user/{id}", async (IDynamoDBContext context, string id, [FromBody] User user) =>
{
    var userDb = await context.LoadAsync<User>(id);
    
    if (userDb is null) return Results.NotFound();

    userDb.Name = user.Name;

    await context.SaveAsync(userDb);
    return Results.NoContent();
});

app.MapDelete("api/user/{id}", async (IDynamoDBContext context, string id) =>
{
    var user = await context.LoadAsync<User>(id);
    
    if (user is null) return Results.NotFound();
    
    await context.DeleteAsync<User>(id);
    return Results.NoContent();
});

app.Run();
return;

AmazonDynamoDBClient ConfigDynamoDb(WebApplicationBuilder webApplicationBuilder)
{
    var awsConfigSection = webApplicationBuilder.Configuration.GetSection("AwsConfig");
    var url = awsConfigSection["ServiceURL"];
    var key = awsConfigSection["AccessKey"];
    var secret = awsConfigSection["SecretKey"];

    var config = new AmazonDynamoDBConfig
    {
        ServiceURL = url
    };

    var credentials = new BasicAWSCredentials(key, secret);
    var amazonDynamoDbClient = new AmazonDynamoDBClient(credentials, config);
    webApplicationBuilder.Services.AddSingleton<IAmazonDynamoDB>(amazonDynamoDbClient);

    var context = new DynamoDBContext(amazonDynamoDbClient);
    webApplicationBuilder.Services.AddSingleton<IDynamoDBContext>(context);

    return amazonDynamoDbClient;
}

async Task CreateTableIfNotExists(IAmazonDynamoDB client)
{
    var listTable = new List<string>()
    {
        "User",
    };

    foreach (var tableName in listTable)
    {
        try
        {
            var tableResponse = await client.DescribeTableAsync(tableName);
        }
        catch (ResourceNotFoundException)
        {
            var createTableRequest = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition("Id", ScalarAttributeType.S)
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement("Id", KeyType.HASH)
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                }
            };

            await client.CreateTableAsync(createTableRequest);

            var tableStatus = "CREATING";
            while (tableStatus == "CREATING")
            {
                await Task.Delay(1000);
                var response = await client.DescribeTableAsync(tableName);
                tableStatus = response.Table.TableStatus;
            }
        }
    }
}