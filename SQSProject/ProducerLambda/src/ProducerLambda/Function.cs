using Amazon.Lambda.Core;
using Amazon.SQS;
using Amazon.SQS.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ProducerLambda;

public class Function
{
    private static readonly string QueueUrl = Environment.GetEnvironmentVariable("QUEUE_URL");
    private static readonly AmazonSQSClient sqsClient = new AmazonSQSClient();

    public async Task FunctionHandler(string input, ILambdaContext context)
    {
        var response = await sqsClient.SendMessageAsync(new SendMessageRequest
        {
            QueueUrl = QueueUrl,
            MessageBody = input
        });

        context.Logger.LogLine($"Sent message ID: {response.MessageId}");
    }
}
