using System.Globalization;

namespace Sepid.DeviceManagerTest.Common.LanguageService
{
    public class LanguageInfo : ILanguageInfo
    {
        public string LanguageCode { get; set; }

        public CultureInfo CultureInfo { get; set; }
    }
}