using System;
using System.IO;
using System.Management;
using System.Text;
using DeviceId;
using DeviceId.Encoders;
using DeviceId.Formatters;
using Serilog;


namespace Sepid.DeviceManagerTest.Common.Helper
{
    public class Utils
    {
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            TimeSpan diff = date - origin;

            return Math.Floor(diff.TotalSeconds);
        }

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }

        public static string CronExpressionMaker(TimeSpan timeSpan)
        {
            StringBuilder expression = new StringBuilder();

            if (timeSpan.Minutes != 0 && timeSpan.Seconds == 0)
                expression.Append($"0 */{timeSpan.Minutes} * * * *");

            if (timeSpan.Seconds != 0 && timeSpan.Minutes == 0)
                expression.Append($"*/{timeSpan.Seconds} * * * * *");

            else
            {
                expression.Append($"{timeSpan.Seconds} */{timeSpan.Minutes} * * * *");
            }

            return expression.ToString();
        }

        public static string GetMacAddress()
        {
            string macSrc = "";
            string macAddress = "";

            foreach (System.Net.NetworkInformation.NetworkInterface nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                {
                    macSrc += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            while (macSrc.Length < 12)
            {
                macSrc = macSrc.Insert(0, "0");
            }

            for (int i = 0; i < 11; i++)
            {
                if (0 == (i % 2))
                {
                    if (i == 10)
                    {
                        macAddress = macAddress.Insert(macAddress.Length, macSrc.Substring(i, 2));
                    }
                    else
                    {
                        macAddress = macAddress.Insert(macAddress.Length, macSrc.Substring(i, 2)) + "-";
                    }
                }
            }
            return macAddress;
        }

       
        public static string GetSystemId()=> new DeviceIdBuilder()
            .AddSystemUUID()
            .UseFormatter(new StringDeviceIdFormatter(new PlainTextDeviceIdComponentEncoder()))
            .ToString();

       


        public static string ReadFile(string path)
        {

            return File.ReadAllText(path);  
        }
    }
}