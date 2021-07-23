using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Common.RabbitMq
{
    public class BusSubscribe : IBusSubscribe
    {
        private readonly IRabbitMqConnection _connection;

        public BusSubscribe(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public void Subscribe(string queueName, Func<string, IDictionary<string, object>, Task<bool>> callBack)
        {
            var factory = new ConnectionFactory
            {
                HostName = _connection.Server,
                UserName = _connection.UserName,
                Password = _connection.Password
            };

            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare(queueName, true, false, false, null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;

                var message = Encoding.UTF8.GetString(body.ToArray());

                var success = await callBack.Invoke(message, ea.BasicProperties.Headers);

                if (success)
                    channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queueName, false, consumer);
        }
    }
}