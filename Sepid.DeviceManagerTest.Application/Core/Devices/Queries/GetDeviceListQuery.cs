using System;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Devices.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.Helper;

namespace Sepid.DeviceManagerTest.Application.Core.Devices.Queries
{
    public class GetDeviceListQuery : IRequest<List<DeviceDto>>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string DeviceModelIds { get; set; }
        public string SdkTypeIds { get; set; }
        public bool? IsConnected { get; set; }
        public string Query { get; set; }
    }

    public class GetDeviceListQueryHandler : IRequestHandler<GetDeviceListQuery, List<DeviceDto>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessGroupService _userAccessGroupService;

        public GetDeviceListQueryHandler(IDeviceManagerContext context, IMapper mapper, IUserAccessGroupService userAccessGroupService)
        {
            _context = context;
            _mapper = mapper;
            _userAccessGroupService = userAccessGroupService;
        }

        public async Task<List<DeviceDto>> Handle(GetDeviceListQuery request, CancellationToken cancellationToken)
        {
            var ids = await _userAccessGroupService.GetAllDeviceAccessId();

            IQueryable<Device> devices = _context.Devices
                .Include(x => x.DeviceModel);

            if (!string.IsNullOrWhiteSpace(request.Query))
                devices = devices.Where(x => x.Name.Contains(request.Query)
                                             || x.Serial.Contains(request.Query)
                                             || x.Ip.Contains(request.Query));

            if (request.FromDate.HasValue || request.ToDate.HasValue)
            {
                request.FromDate ??= new DateTime();
                request.ToDate ??= DateTime.Now;

                devices = devices.Where(x => x.LastLogRetrieve >= request.FromDate && x.LastLogRetrieve <= request.ToDate);
            }

            if (ids.Any())
            {
                devices = devices.Where(x => ids.Contains(x.Id));
            }

            if (request.IsConnected.HasValue)
                devices = devices.Where(x => x.IsConnected == request.IsConnected);

            if (!string.IsNullOrWhiteSpace(request.DeviceModelIds))
            {
                HashSet<long> deviceModelIds =
                    new HashSet<long>(ParameterSeparator<long>.SeparateLong(request.DeviceModelIds));

                devices = devices.Where(x => deviceModelIds.Contains(x.DeviceModelId.Value));
            }

            if (!string.IsNullOrWhiteSpace(request.SdkTypeIds))
            {
                HashSet<SdkType> sdkTypeIds =
                    new HashSet<SdkType>(ParameterSeparator<SdkType>.SeparateEnum(request.SdkTypeIds));

                devices = devices.Where(x => sdkTypeIds.Contains(x.DeviceModel.SdkType));
            }

            return _mapper.Map<List<DeviceDto>>(await devices.ToListAsync(cancellationToken));
        }
    }
}