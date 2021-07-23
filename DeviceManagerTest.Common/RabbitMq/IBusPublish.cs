namespace Sepid.DeviceManagerTest.Common.RabbitMq
{
    public interface IBusPublish
    {
        void Send(string queueName, string data);
    }
}