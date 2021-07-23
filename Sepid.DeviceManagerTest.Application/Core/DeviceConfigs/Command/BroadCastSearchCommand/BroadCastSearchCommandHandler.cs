using System;
using AutoMapper;
using MediatR;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Dto;
using Sepid.DeviceManagerTest.Common.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Sepid.DeviceManagerTest.Common.DeviceServices;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.BroadCastSearchCommand
{
    public class BroadCastSearchCommandHandler : IRequestHandler<BroadCastSearchCommand, List<DeviceSearchDto>>
    {
        private readonly IEnumerable<IDeviceOperationServices> _services;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        

        public BroadCastSearchCommandHandler(IEnumerable<IDeviceOperationServices> services, IMapper mapper, IDistributedCache cache)
        {
            _services = services;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<DeviceSearchDto>> Handle(BroadCastSearchCommand request, CancellationToken cancellationToken)
        {
            List<DeviceInfo> deviceSearch = new List<DeviceInfo>();

            var search = await _cache.GetStringAsync("Devices", token: cancellationToken);

            if (search is null)
            {
                foreach (var service in _services)
                {
                    var devices = service.BroadCastSearch();

                    devices.ForEach(device =>
                    {
                        deviceSearch.Add(device);
                    });
                }

                await _cache.SetStringAsync("Devices", JsonConvert.SerializeObject(deviceSearch),
                    new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(30)), cancellationToken);

            }
            else
                deviceSearch = JsonConvert.DeserializeObject<List<DeviceInfo>>(search);



            return _mapper.Map<List<DeviceSearchDto>>(deviceSearch);
        }
    }
}