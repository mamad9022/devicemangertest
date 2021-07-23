using System.Collections.Generic;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Stats.Dto;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Sepid.DeviceManagerTest.Common.Helper;

namespace Sepid.DeviceManagerTest.Application.Core.Stats.Queries
{
    public class GetStatsQuery : IRequest<StatDto>
    {

    }

    public class GetStatsQueryHandler : IRequestHandler<GetStatsQuery, StatDto>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IDistributedCache _cache;


        public GetStatsQueryHandler(IDeviceManagerContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<StatDto> Handle(GetStatsQuery request, CancellationToken cancellationToken)
        {
            var devices = await _cache.GetRecordAsync<List<OverFlowLogDevice>>("overflowDevice", cancellationToken);


            if (devices is null)
            {
                devices = await _context.Devices.Where(x => x.IsConnected && x.CurrentLogCount != null).Select(x => new OverFlowLogDevice
                {
                    Name = x.Name,
                    Serial = x.Serial,
                    Image = x.DeviceModel.Image,
                    TotalLog = x.DeviceModel.TotalLog,
                    CurrentLog = x.CurrentLogCount,
                    Percentage = (((double)x.CurrentLogCount.Value * 100) / (double)x.DeviceModel.TotalLog)
                }).OrderByDescending(x => x.Percentage).Take(5)
                   .ToListAsync(cancellationToken)
                   .ConfigureAwait(false);

                await _cache.SetRecordAsync("overflowDevice", devices, cancellationToken);
            }



            return new StatDto
            {
                ActiveDevice = await _context.Devices.CountAsync(x => x.IsConnected, cancellationToken),
                DeactivateDevice = await _context.Devices.CountAsync(x => x.IsConnected == false, cancellationToken),
                TotalDevice = await _context.Devices.CountAsync(cancellationToken),
                overFlowLogDevices = devices
            };
        }
    }
}
