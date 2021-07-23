using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Sepid.DeviceManagerTest.Common.Helper
{
    public class EnumConvertor
    {
        public static string GetDisplayName(System.Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                ?.GetName();
        }
    }
}