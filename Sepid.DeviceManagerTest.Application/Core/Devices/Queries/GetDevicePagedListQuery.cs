using System;
using System.Collections.Generic;
using AutoMapper;
using MediatR;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Pagination;
using Sepid.DeviceManagerTest.Application.Core.Devices.Dto;
using Sepid.DeviceManagerTest.Application.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.Helper;
using PagingOptions = Sepid.DeviceManagerTest.Application.Common.Pagination.PagingOptions;

namespace Sepid.DeviceManagerTest.Application.Core.Devices.Queries
{
    public class GetDevicePagedListQuery : PagingOptions, IRequest<PagedList<DeviceListDto>>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string DeviceModelIds { get; set; }
        public string SdkTypeIds { get; set; }
        public bool? IsConnected { get; set; }
        public bool? IsVital { get; set; }
    }

    public class GetDevicePagedListQueryHandler : PagingService<Device>, IRequestHandler<GetDevicePagedListQuery, PagedList<DeviceListDto>>
    {
        private readonly IMapper _mapper;
        private readonly IDeviceManagerContext _context;
        private readonly IUserAccessGroupService _userAccessGroupService;

        public GetDevicePagedListQueryHandler(IMapper mapper, IDeviceManagerContext context, IUserAccessGroupService userAccessGroupService)
        {
            _mapper = mapper;
            _context = context;
            _userAccessGroupService = userAccessGroupService;
        }

        public async Task<PagedList<DeviceListDto>> Handle(GetDevicePagedListQuery request, CancellationToken cancellationToken)
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


            if (request.IsVital.HasValue)
                devices = devices.Where(x => x.IsVital == request.IsVital);


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

            var deviceList = await GetPagedAsync(request.Page, request.Limit, devices, cancellationToken);

            return deviceList.MapTo<DeviceListDto>(_mapper);
        }
    }
}