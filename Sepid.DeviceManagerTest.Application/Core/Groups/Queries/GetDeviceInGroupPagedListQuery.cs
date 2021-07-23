using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Pagination;
using Sepid.DeviceManagerTest.Application.Core.Devices.Dto;
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Queries
{
    public class GetDeviceInGroupPagedListQuery : PagingOptions, IRequest<Result<PagedList<DeviceListDto>>>
    {
        public int GroupId { get; set; }
    }

    public class
        GetDeviceInGroupPagedListQueryHandler : PagingService<Device>, IRequestHandler<GetDeviceInGroupPagedListQuery,
            Result<PagedList<DeviceListDto>>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public GetDeviceInGroupPagedListQueryHandler(IDeviceManagerContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result<PagedList<DeviceListDto>>> Handle(GetDeviceInGroupPagedListQuery request,
            CancellationToken cancellationToken)
        {
            if (await _context.Groups.AnyAsync(x => x.Id == request.GroupId, cancellationToken) == false)
                return Result<PagedList<DeviceListDto>>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.GroupNotFound, cancellationToken))));


            var devices = _context.Devices
                .Include(x => x.DeviceModel)
                .Include(x => x.DeviceInGroups)
                .Where(x => x.DeviceInGroups.Any(g => g.GroupId == request.GroupId));

            if (!string.IsNullOrWhiteSpace(request.Query))
                devices = devices.Where(x => x.Name.Contains(request.Query)
                                             || x.Serial.Contains(request.Query)
                                             || x.Ip.Contains(request.Query));

            var deviceList = await GetPagedAsync(request.Page, request.Limit, devices, cancellationToken);


            return Result<PagedList<DeviceListDto>>.SuccessFul(deviceList.MapTo<DeviceListDto>(_mapper));
        }
    }
}