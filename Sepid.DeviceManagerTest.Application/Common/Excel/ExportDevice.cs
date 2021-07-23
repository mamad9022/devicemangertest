using ClosedXML.Excel;
using DNTPersianUtils.Core;
using Sepid.DeviceManagerTest.Application.Common.Environment;
using Sepid.DeviceManagerTest.Application.Core.Devices.Dto;
using Sepid.DeviceManagerTest.Application.Core.Devices.Dto;
using Serilog;
using System;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Common.Excel
{
    public static class ExportDevice
    {
        public static string DeviceList(List<DeviceDto> devices)
        {
            try
            {
                using var workbook = new XLWorkbook();

                IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Devices");

                worksheet.Cell(1, 1).Value = "ردیف";
                worksheet.Cell(1, 2).Value = "نام";
                worksheet.Cell(1, 3).Value = "شماره سریال";
                worksheet.Cell(1, 4).Value = "ip";
                worksheet.Cell(1, 5).Value = "پورت";
                worksheet.Cell(1, 6).Value = "وضعیت";
                worksheet.Cell(1, 7).Value = "استفاده از dhcp";
                worksheet.Cell(1, 8).Value = "کد دستگاه";

                for (int index = 1; index <= devices.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = index;
                    worksheet.Cell(index + 1, 2).Value = devices[index - 1].Name;
                    worksheet.Cell(index + 1, 3).Value = devices[index - 1].Serial;
                    worksheet.Cell(index + 1, 4).Value = devices[index - 1].Ip;
                    worksheet.Cell(index + 1, 5).Value = devices[index - 1].Port;
                    worksheet.Cell(index + 1, 6).Value = devices[index - 1].IsConnected ? "فعال" : "غیر فعال";
                    worksheet.Cell(index + 1, 7).Value = devices[index - 1].UseDhcp ? "بله" : "خیر";
                    worksheet.Cell(index + 1, 8).Value = devices[index - 1].Code;
                }

                var uniqueName = $" لیست دستگاه - {DateTime.Now.ToLongPersianDateString()}.xlsx";
                var fileName = $"{ApplicationStaticPath.Documents}/{uniqueName}";

                workbook.SaveAs(fileName);

                return $"{ApplicationStaticPath.Clients.Document}/{uniqueName}";
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, ex.Message);
                return string.Empty;
            }
        }
    }
}