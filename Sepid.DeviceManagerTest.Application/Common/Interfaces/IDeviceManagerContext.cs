using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sepid.DeviceManagerTest.Application.Models;

namespace Sepid.DeviceManagerTest.Application.Common.Interfaces
{
    public interface IDeviceManagerContext
    {
        DbSet<Device> Devices { get; set; }

        DbSet<DeviceModel> DeviceModels { get; set; }

        DbSet<TransferLog> TransferLogs { get; set; }

        DbSet<Group> Groups { get; set; }

        DbSet<DeviceInGroup> DeviceInGroups { get; set; }

        DbSet<Setting> Settings { get; set; }

        DbSet<UserAccessGroup> UserAccessGroups { get; set; }

        ChangeTracker ChangeTracker { get; }

        Task SaveAsync(CancellationToken cancellationToken);

        void Save();
    }
}