namespace Sepid.DeviceManagerTest.Client.Models
{
    public class DeleteUserCommand
    {
        public int DeviceId { get; set; }
        public string PersonCode { get; set; }
        public string Name { get; set; }
    }
}