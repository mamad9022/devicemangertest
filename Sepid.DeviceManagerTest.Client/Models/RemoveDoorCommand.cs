using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Client.Models
{
    public class RemoveDoorCommand
    {
        public int DeviceId { get; set; }
        public List<int> DoorIds { get; set; }
    }
}