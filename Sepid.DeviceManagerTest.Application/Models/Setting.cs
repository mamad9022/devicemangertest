namespace Sepid.DeviceManagerTest.Application.Models
{
    public class Setting
    {
        public int Id { get; set; }

        public int RetryFailedTransferNumber { get; set; }

        public int FingerPrintQuality { get; set; }

        public int AutoClearLogPercentage { get; set; }

        public bool EnableClearLog { get; set; }

        public string License { get; set; }

        public int VitalDevice { get; set; }
    }
}