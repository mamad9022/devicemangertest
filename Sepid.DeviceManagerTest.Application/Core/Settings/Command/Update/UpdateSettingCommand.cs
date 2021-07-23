using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Settings.Command.Update
{
    public class UpdateSettingCommand : IRequest<Result>
    {
        public int RetryFailedTransferNumber { get; set; }

        public int FingerPrintQuality { get; set; }

        public int AutoClearLogPercentage { get; set; }

        public bool EnableClearLog { get; set; }

        public string License { get; set; }

        public string SystemId { get; set; }

        public int VitalDevice { get; set; }
    }
}