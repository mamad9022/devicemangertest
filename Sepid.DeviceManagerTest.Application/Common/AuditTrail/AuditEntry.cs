﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.Helper;

namespace Sepid.DeviceManagerTest.Application.Common.AuditTrail
{
    public class AuditEntry
    {
        public EntityEntry Entry { get; }
        public AuditType AuditType { get; set; }
        public string AuditUser { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<string> ChangedColumns { get; } = new List<string>();

        public AuditEntry(EntityEntry entry, string auditUser)
        {
            Entry = entry;
            AuditUser = auditUser;
            SetChanges();
        }

        private void SetChanges()
        {
            TableName = Entry.Entity.GetType().Name;

            foreach (PropertyEntry property in Entry.Properties)
            {
                string propertyName = property.Metadata.Name;
                string dbColumnName = propertyName;

                if (property.Metadata.IsPrimaryKey())
                {
                    KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (Entry.State)
                {
                    case EntityState.Added:
                        NewValues[propertyName] = property.CurrentValue;
                        AuditType = AuditType.Create;
                        break;

                    case EntityState.Deleted:
                        OldValues[propertyName] = property.OriginalValue;
                        AuditType = AuditType.Delete;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            ChangedColumns.Add(dbColumnName);

                            OldValues[propertyName] = property.OriginalValue;
                            NewValues[propertyName] = property.CurrentValue;
                            AuditType = AuditType.Update;
                        }

                        break;
                }
            }
        }

        //public Audit ToAudit(IRequestMeta requestMeta, ICurrentUserService currentUserService)
        //{
        //    var audit = new Audit
        //    {
        //        Id = Guid.NewGuid(),
        //        AuditDateTimeUtc = DateTime.UtcNow,
        //        AuditType = AuditType.ToString(),
        //        AuditUser = AuditUser,
        //        TableName = TableName,
        //        KeyValues = JsonConvert.SerializeObject(KeyValues),
        //        OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
        //        NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues),
        //        ChangedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns),
        //        Ip = requestMeta.Ip,
        //        Browser = requestMeta.Browser,
        //        Os = requestMeta.Os,
        //        AuditFullName = currentUserService.FullName
        //    };

        //    return audit;
        //}
    }
}