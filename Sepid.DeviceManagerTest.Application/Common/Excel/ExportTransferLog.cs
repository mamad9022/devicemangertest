using System;
using System.Collections.Generic;
using ClosedXML.Excel;
using DNTPersianUtils.Core;
using Sepid.DeviceManagerTest.Application.Common.Environment;
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.Helper;
using Serilog;

namespace Sepid.DeviceManagerTest.Application.Common.Excel
{
    public static class ExportTransferLog
    {
        public static string TransferLog(List<TransferLog> devices)
        {
            try
            {
                using var workbook = new XLWorkbook();

                IXLWorksheet worksheet =
                    workbook.Worksheets.Add("TransferLog");

                worksheet.Cell(1, 1).Value = "ردیف";
                worksheet.Cell(1, 2).Value = "نام";
                worksheet.Cell(1, 3).Value = "کد پرسنلی";
                worksheet.Cell(1, 4).Value = "تعداد تلاش";
                worksheet.Cell(1, 5).Value = "سریال دستگاه";
                worksheet.Cell(1, 6).Value = "محل دستگاه";
                worksheet.Cell(1, 7).Value = "پیغام خطا";
                worksheet.Cell(1, 8).Value = "نوع انتفال";
                worksheet.Cell(1, 9).Value = "وضعیت انتقال";
                worksheet.Cell(1, 10).Value = "تاریخ انتقال";


                for (int index = 1; index <= devices.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = index;
                    worksheet.Cell(index + 1, 2).Value = devices[index - 1].Description.Split("-")[0];
                    worksheet.Cell(index + 1, 3).Value = devices[index - 1].Description.Split("-")[1];
                    worksheet.Cell(index + 1, 4).Value = devices[index - 1].Retry;
                    worksheet.Cell(index + 1, 5).Value = devices[index - 1].Device.Serial;
                    worksheet.Cell(index + 1, 6).Value = devices[index - 1].Device.Name;
                    worksheet.Cell(index + 1, 7).Value = devices[index - 1].ErrorMessage;
                    worksheet.Cell(index + 1, 8).Value = EnumConvertor.GetDisplayName(devices[index - 1].TransferLogType);
                    worksheet.Cell(index + 1, 9).Value = devices[index - 1].IsSuccess ? " موفق" : "ناموفق";
                    worksheet.Cell(index + 1, 10).Value = devices[index - 1].CreateDate.ToLongPersianDateTimeString();

                }

                var uniqueName = $" لیست گزارش انتقال ستجه - {DateTime.Now.ToLongPersianDateString()}.xlsx";
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