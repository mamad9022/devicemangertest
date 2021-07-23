namespace Sepid.DeviceManagerTest.Common.Options
{
    public class DataStorage : IDataStorage
    {
        public string Path { get; set; }

        public string HangFirePath { get; set; }
    }

    public interface IDataStorage
    {
        string Path { get; set; }
        string HangFirePath { get; set; }
    }
}