🔷 Step 1: Create the SQS Queue
Go to Amazon SQS in AWS Console.

Click “Create queue”.

Choose Standard Queue.

Name it (e.g., MyQueue).

Leave other settings as default.

Click Create Queue.

🔶 Step 2: Create Producer Lambda Function
Go to AWS Lambda > Create function.

Choose Author from scratch.

Function name: ProducerLambda

Runtime: .NET 6 (C#) or Python/Node.js (if testing quickly)

Click Create Function.

📌 Add Environment Variable:
Key: QUEUE_URL

Value: The full SQS URL (visible in SQS Console under the queue's details)

🔑 Add IAM Permission:
Go to Configuration > Permissions tab.

Click Execution Role > Role name.

Under Permissions > Add permissions > Attach policies.

Search and attach: AmazonSQSFullAccess (or better: custom policy with sqs:SendMessage only)

🔷 Step 3: Create Consumer Lambda Function
Create new Lambda (name: ConsumerLambda)

Runtime: same as above

Click Create Function

🔗 Add SQS Trigger:
Go to Configuration > Triggers

Click Add trigger

Choose SQS

Select MyQueue

Click Add

🚨 This creates an event source mapping, connecting SQS → Lambda

✅ End-to-End Test
Open Producer Lambda > Test tab.

Enter test event like:

"Hello Queue from Producer"

Run the test.

Check Consumer Lambda logs in CloudWatch Logs to see if the message was processed.