using System;
using Sepid.DeviceManagerTest.Common.Enum;

namespace Sepid.DeviceManagerTest.Application.Core.Report.Dto
{
    public class TransferLogDto
    {
        public Guid Id { get; set; }

        public int Retry { get; set; }

        public TransferLogType TransferLogType { get; set; }

        public string Description { get; set; }

        public string ErrorMessage { get; set; }

        public string Reason { get; set; }

        public bool IsSuccess { get; set; }

        public string DeviceName { get; set; }

        public string DeviceSerial { get; set; }

        public DateTime CreateDate { get; set; }
    }
}