using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Report.Dto;
using Sepid.DeviceManagerTest.Common.Helper;

namespace Sepid.DeviceManagerTest.Application.Core.Report.Queries
{
    public class GetDeviceStatsQuery : IRequest<DeviceStatsViewModel>
    {
        
    }

    public class GetDeviceStatsQueryHandler : IRequestHandler<GetDeviceStatsQuery,DeviceStatsViewModel>
    {
        private readonly IDeviceManagerContext _context;

        public GetDeviceStatsQueryHandler(IDeviceManagerContext context)
        {
            _context = context;
        }

        public async Task<DeviceStatsViewModel> Handle(GetDeviceStatsQuery request, CancellationToken cancellationToken)
        {
            var totalDevice = await _context.Devices.CountAsync(cancellationToken).ConfigureAwait(false);


            var activeDevice = await _context.Devices
                .GroupBy(x => x.IsConnected)
                .Select(x => new DeviceStatsDto
                {
                    Name = x.Key ? "فعال" : "غیر فعال",
                    Count = x.Count(),
                    Percentage = ((double)(x.Count() * 100)/totalDevice)

                }).ToListAsync(cancellationToken).ConfigureAwait(false);


            var deviceBrand = await _context.Devices
                .Include(x => x.DeviceModel)
                .GroupBy(x => x.DeviceModel.SdkType)
                .Select(x=>new DeviceStatsDto
                {
                    Name = EnumConvertor.GetDisplayName(x.Key),
                    Count = x.Count(),
                    Percentage = ((double)(x.Count() * 100)/totalDevice)
                }).ToListAsync(cancellationToken)
                .ConfigureAwait(false);




            return new DeviceStatsViewModel
            {
                TotalDevice = totalDevice,
                ConnectedDevices = activeDevice,
                DeviceBrands = deviceBrand
            };
        }
    }
}