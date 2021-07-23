using System.Globalization;

namespace Sepid.DeviceManagerTest.Common.LanguageService
{
    public interface ILanguageInfo
    {
        string LanguageCode { get; set; }

        CultureInfo CultureInfo { get; set; }
    }
}