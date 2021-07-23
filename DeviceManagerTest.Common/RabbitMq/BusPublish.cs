using RabbitMQ.Client;
using System.Text;

namespace Sepid.DeviceManagerTest.Common.RabbitMq
{
    public class BusPublish : IBusPublish
    {
        private readonly IRabbitMqConnection _connection;

        public BusPublish(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public void Send(string queueName, string data)
        {
            var factory = new ConnectionFactory
            {
                HostName = _connection.Server,
                UserName = _connection.UserName,
                Password = _connection.Password
            };

            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();
            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2;

            channel.ExchangeDeclare(queueName, ExchangeType.Fanout);

            channel.QueueDeclare(queueName, true, false, false, null);

            channel.QueueBind(queueName, queueName, $"{queueName}-Key");

            channel.BasicPublish(queueName, $"{queueName}-Key", properties, Encoding.UTF8.GetBytes(data));

            channel.Dispose();
            connection.Dispose();
        }
    }
}