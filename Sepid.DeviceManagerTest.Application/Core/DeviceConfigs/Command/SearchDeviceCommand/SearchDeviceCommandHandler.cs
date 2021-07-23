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
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.SearchDeviceCommand
{
    public class SearchDeviceCommandHandler : IRequestHandler<SearchDeviceCommand, DeviceSearchDto>
    {
        private readonly IEnumerable<IDeviceOperationServices> _services;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public SearchDeviceCommandHandler(IEnumerable<IDeviceOperationServices> services, IMapper mapper, IDistributedCache cache)
        {
            _services = services;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<DeviceSearchDto> Handle(SearchDeviceCommand request, CancellationToken cancellationToken)
        {
            DeviceInfo deviceSearch = new DeviceInfo();

            var search = await _cache.GetStringAsync(JsonSerializer.Serialize(request), token: cancellationToken);

            if (search is null)
            {
                foreach (var service in _services)
                {
                    var devices = service.Search(new BaseDeviceInfoDto
                    {
                        Ip = request.Ip,
                        Port = request.Port
                    });
                    if (devices != null)
                    {
                        deviceSearch = devices;

                        await _cache.SetStringAsync(JsonSerializer.Serialize(request), JsonConvert.SerializeObject(deviceSearch),
                            new DistributedCacheEntryOptions()
                                .SetSlidingExpiration(TimeSpan.FromSeconds(30)), cancellationToken);
                        
                        break;
                    }
                }


            }
            else
                deviceSearch = JsonConvert.DeserializeObject<DeviceInfo>(search);



            return _mapper.Map<DeviceSearchDto>(deviceSearch);
        }
    }
}