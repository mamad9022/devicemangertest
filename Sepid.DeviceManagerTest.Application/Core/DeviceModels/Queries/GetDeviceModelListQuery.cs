using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Dto;
using Sepid.DeviceManagerTest.Application.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Common.Helper;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceModels.Queries
{
    public class GetDeviceModelListQuery : IRequest<List<DeviceModelDto>>
    {
    }

    public class GetDeviceModelListQueryHandler : IRequestHandler<GetDeviceModelListQuery, List<DeviceModelDto>>
    {
        private readonly IMapper _mapper;
        private readonly IDeviceManagerContext _context;
        private readonly IDistributedCache _cache;

        public GetDeviceModelListQueryHandler(IMapper mapper, IDeviceManagerContext context, IDistributedCache cache)
        {
            _mapper = mapper;
            _context = context;
            _cache = cache;
        }

        public async Task<List<DeviceModelDto>> Handle(GetDeviceModelListQuery request, CancellationToken cancellationToken)
        {


            var allDeviceModels = await _cache.GetRecordAsync<List<DeviceModel>>("getAllDeviceModels", cancellationToken);

            if (allDeviceModels is null)
            {
                allDeviceModels = await _context.DeviceModels.ToListAsync(cancellationToken);

                await _cache.SetRecordAsync("getAllDeviceModels", allDeviceModels, cancellationToken, TimeSpan.FromDays(100));
            }

            return _mapper.Map<List<DeviceModelDto>>(allDeviceModels);
        }
    }
}