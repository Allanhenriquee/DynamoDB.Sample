using Amazon.DynamoDBv2.DataModel;

namespace DynamoDB.Sample.API.Models;

[DynamoDBTable("User")]
public class User
{
    [DynamoDBHashKey]
    public required string Id { get; set; }

    [DynamoDBProperty]
    public required string Name { get; set; }
}