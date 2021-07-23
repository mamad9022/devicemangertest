namespace Sepid.DeviceManagerTest.Common.Helper
{
    public class ApiMessage
    {
        public ApiMessage()
        {
        }

        public ApiMessage(string error)
        {
            message = error;
        }

        public string message { get; set; }

        public string Detail { get; set; }
    }
}