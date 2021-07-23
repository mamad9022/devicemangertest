using System.Collections.Generic;
using System.Linq;
using Sepid.DeviceManagerTest.Common.Enum;

namespace Sepid.DeviceManagerTest.Common.Helper
{
    public class ParameterSeparator<T>
    {
        public static List<int> SeparateInt(string param)
        {
            List<int> ids = new List<int>();

            if (string.IsNullOrWhiteSpace(param))
                return ids;

            var list = param.Split(',')
                .Where(x => x.Trim().Length > 0)
                .Select(x => x.Trim()).ToList();

            list.ForEach(x =>
            {
                if (int.TryParse(x, out var id))
                    ids.Add(id);
            });

            return ids;
        }

        public static List<long> SeparateLong(string param)
        {
            List<long> ids = new List<long>();

            if (string.IsNullOrWhiteSpace(param))
                return ids;

            var list = param.Split(',')
                .Where(x => x.Trim().Length > 0)
                .Select(x => x.Trim()).ToList();

            list.ForEach(x =>
            {
                if (long.TryParse(x, out var id))
                    ids.Add(id);
            });

            return ids;
        }

        public static List<SdkType> SeparateEnum(string param)
        {
            List<SdkType> ids = new List<SdkType>();

            if (string.IsNullOrWhiteSpace(param))
                return ids;

            var list = param.Split(',')
                .Where(x => x.Trim().Length > 0)
                .Select(x => x.Trim()).ToList();

            list.ForEach(x =>
            {
                if (System.Enum.TryParse<SdkType>(x, out var id))
                    ids.Add(id);
            });

            return ids;
        }
    }
}