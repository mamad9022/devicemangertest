namespace Sepid.DeviceManagerTest.Common.Dto
{
    public class ScanResultDto
    {
        public byte[] TemplateData { get; set; }

        public byte[] TemplateImage { get; set; }

        public int? Quality { get; set; }

        public string Template { get; set; }
    }
}