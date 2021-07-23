using System;

namespace Sepid.DeviceManagerTest.Application.Common.AuditTrail
{
    public class Audit
    {
        public Guid Id { get; set; }
        public DateTime AuditDateTimeUtc { get; set; }
        public string AuditType { get; set; }
        public string AuditUser { get; set; }
        public string AuditFullName { get; set; }
        public string Ip { get; set; }
        public string Browser { get; set; }
        public string Os { get; set; }
        public string TableName { get; set; }
        public string KeyValues { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string ChangedColumns { get; set; }
    }
}