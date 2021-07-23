using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Models;
using System.Threading;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Application.Common.AuditTrail;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.RabbitMq;

namespace Sepid.DeviceManagerTest.Persistence.Context
{
    public class DeviceManagerContext : DbContext, IDeviceManagerContext
    {
        //private readonly ICurrentUserService _currentUserService;
        private readonly IRequestMeta _requestMeta;
        private readonly IBusPublish _busPublish;

        public DeviceManagerContext()
        {
            
        }

        public DeviceManagerContext(DbContextOptions<DeviceManagerContext> options, IRequestMeta requestMeta, IBusPublish busPublish) : base(options)
        {
            //_currentUserService = currentUserService;
            _requestMeta = requestMeta;
            _busPublish = busPublish;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=DESKTOP-HBEGIIU\SQLEXPRESS;Initial Catalog =DeviceManagerContext;MultipleActiveResultSets=true;User ID=sa;Password=AbC123_-");
            }
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceModel> DeviceModels { get; set; }
        public DbSet<TransferLog> TransferLogs { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<DeviceInGroup> DeviceInGroups { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<UserAccessGroup> UserAccessGroups { get; set; }


        public Task SaveAsync(CancellationToken cancellationToken)
        {
            //new AuditHelper(this, _currentUserService, _requestMeta, _busPublish).AddAuditLogs();
            return base.SaveChangesAsync(cancellationToken);
        }


        public void Save() => base.SaveChanges();

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeviceManagerContext).Assembly);
    }
}