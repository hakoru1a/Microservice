// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");


var connectionFactory = new ConnectionFactory
{
    HostName = "localhost",
};


var connection = await connectionFactory.CreateConnectionAsync();

using var channel = await connection.CreateChannelAsync();

_ = channel.QueueDeclareAsync("orders", exclusive: false, autoDelete: true);

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (model, ea) =>
{
    try
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [x] Received {0}", message);

        // Process your message here

        // Acknowledge the message has been processed
        await channel.BasicAckAsync(ea.DeliveryTag, false);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error processing message: {ex.Message}");
        // Negative acknowledgment - message will be requeued
        await channel.BasicNackAsync(ea.DeliveryTag, false, true);
    }
};

_ = channel.BasicConsumeAsync(queue: "orders", autoAck: false, consumer: consumer);

Console.ReadKey();