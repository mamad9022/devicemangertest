using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.RabbitMq;

namespace Sepid.DeviceManagerTest.Application.Common.AuditTrail
{
    public class AuditHelper
    {
        private readonly IDeviceManagerContext _db;
        //private readonly ICurrentUserService _currentUserService;
        private readonly IRequestMeta _requestMeta;
        private readonly IBusPublish _busPublish;

        public AuditHelper(IDeviceManagerContext db,IRequestMeta requestMeta,
            IBusPublish busPublish)
        {
            _db = db;
            //_currentUserService = currentUserService;
            _requestMeta = requestMeta;
            _busPublish = busPublish;
        }

        public void AddAuditLogs()
        {
            _db.ChangeTracker.DetectChanges();
            List<AuditEntry> auditEntries = new List<AuditEntry>();
            foreach (EntityEntry entry in _db.ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.Entity is UserAccessGroup || entry.Entity is DeviceInGroup ||
                    entry.State == EntityState.Detached ||
                    entry.State == EntityState.Unchanged)
                    continue;

                //var auditEntry = new AuditEntry(entry, _currentUserService.UserName);
                //auditEntries.Add(auditEntry);
            }

            if (auditEntries.Any())
            {
                //var logs = auditEntries.Select(x => x.ToAudit(_requestMeta, _currentUserService));
                //foreach (var log in logs)
                //{
                //    _busPublish.Send("Star-UserActivity", JsonSerializer.Serialize(log));
                //}
            }
        }
    }
}