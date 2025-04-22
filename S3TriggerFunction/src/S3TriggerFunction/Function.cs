using System;
using System.Threading.Tasks;
using Amazon.Lambda.S3Events;
using Amazon.Lambda.Core;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace S3TriggerFunction;

public class Function
{
    private static readonly string topicArn = Environment.GetEnvironmentVariable("TOPIC_ARN"); 
    private readonly IAmazonSimpleNotificationService _snsClient;

    public Function() : this(new AmazonSimpleNotificationServiceClient()) { }

    public Function(IAmazonSimpleNotificationService snsClient)
    {
        _snsClient = snsClient;
    }

    public async Task Handler(S3Event s3Event, ILambdaContext context)
    {
        var record = s3Event.Records?[0];
        if (record == null) return;

        var bucketName = record.S3.Bucket.Name;
        var objectKey = record.S3.Object.Key;

        string message = $"New file uploaded: {objectKey} in bucket {bucketName}";

        await _snsClient.PublishAsync(new PublishRequest
        {
            TopicArn = topicArn,
            Subject = "S3 File Upload Notification",
            Message = message
        });

        context.Logger.LogLine("SNS notification sent successfully.");
    }
}
