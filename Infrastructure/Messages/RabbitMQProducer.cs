using Constracts.Common.Interface;
using Constracts.Messages;
using Infrastructure.Common;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messages
{
    public class RabbitMQProducer : IMessageProducer     
    {
        public ISerializeService _serializable;
        public RabbitMQProducer(ISerializeService serializable)
        {
            _serializable = serializable;
        }
        public async void SendMessage<T>(T message)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
            };

            var connection = await connectionFactory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            _ = channel.QueueDeclareAsync("orders", exclusive: false, autoDelete: true);

            var jsonData = _serializable.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonData);
            
            await channel.BasicPublishAsync(exchange: "", routingKey: "orders", body: body);
        } 
    }
}
