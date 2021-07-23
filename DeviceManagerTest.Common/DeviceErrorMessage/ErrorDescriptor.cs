using System.IO;
using System.Linq;
using System.Text.Json;

namespace Sepid.DeviceManagerTest.Common.DeviceErrorMessage
{

    public static class ErrorDescriptor

    {
        public static ErrorCodeDescription GetError(int key, int versionNumber)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory() + $"/Resources/Zk-V{versionNumber}.json");

            var jsonData = File.ReadAllText(filepath);

            var localization = JsonSerializer.Deserialize<ErrorResources>(jsonData);

            var functionCode = localization.Errors.FirstOrDefault(x => x.Code == key);

            if (functionCode is null)
            {
                return new ErrorCodeDescription
                {
                    Message = "ارور ناشناخته"
                };
            }

            return functionCode;
        }
    }
}