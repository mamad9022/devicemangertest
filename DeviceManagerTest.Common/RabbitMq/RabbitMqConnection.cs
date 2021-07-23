namespace Sepid.DeviceManagerTest.Common.RabbitMq
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        public string Server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public interface IRabbitMqConnection
    {
        string Server { get; set; }

        string UserName { get; set; }

        string Password { get; set; }
    }
}