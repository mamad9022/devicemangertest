using System;
using Sepid.DeviceManagerTest.Common.Enum;

namespace Sepid.DeviceManagerTest.Application.Models
{
    public class TransferLog
    {
        public Guid Id { get; set; }

        public int? DeviceId { get; set; }

        public int Retry { get; set; }

        public TransferLogType TransferLogType { get; set; }

        public string Data { get; set; }

        public string Description { get; set; }

        public string ErrorMessage { get; set; }

        public string Reason { get; set; }

        public bool IsSuccess { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual Device Device { get; set; }
    }
}