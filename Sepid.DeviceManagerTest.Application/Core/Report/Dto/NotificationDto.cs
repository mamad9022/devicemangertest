using Sepid.DeviceManagerTest.Common.Enum;

namespace Sepid.DeviceManagerTest.Application.Core.Report.Dto
{
    public class NotificationDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public LogType LogType { get; set; } = LogType.Info;
    }
}