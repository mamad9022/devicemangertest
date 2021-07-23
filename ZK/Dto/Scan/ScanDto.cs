namespace ZK.Dto.Scan
{
    public class ScanDto
    {
        public int UserId { get; set; }
        public string CardNumber { get; set; }
        public int? Quality { get; set; }
        public byte[] TemplateData { get; set; }
        public byte[] TemplateImage { get; set; }
        public string Template { get; set; }
    }
}
