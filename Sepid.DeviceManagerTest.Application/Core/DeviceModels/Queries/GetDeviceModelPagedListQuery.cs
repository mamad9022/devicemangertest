using AutoMapper;
using MediatR;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Pagination;
using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Dto;
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.Enum;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceModels.Queries
{
    public class GetDeviceModelPagedListQuery : PagingOptions, IRequest<PagedList<DeviceModelDto>>
    {
        public SdkType? Sdk { get; set; }
    }

    public class GetDeviceModelPagedListQueryHandler : PagingService<DeviceModel>, IRequestHandler<GetDeviceModelPagedListQuery, PagedList<DeviceModelDto>>
    {
        private readonly IMapper _mapper;
        private readonly IDeviceManagerContext _context;

        public GetDeviceModelPagedListQueryHandler(IMapper mapper, IDeviceManagerContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PagedList<DeviceModelDto>> Handle(GetDeviceModelPagedListQuery request, CancellationToken cancellationToken)
        {
            IQueryable<DeviceModel> deviceModel = _context.DeviceModels;

            if (!string.IsNullOrWhiteSpace(request.Query))
                deviceModel = deviceModel.Where(x => x.Name.Contains(request.Query));

            if (request.Sdk.HasValue)
                deviceModel = deviceModel.Where(x => x.SdkType == request.Sdk.Value);

            var deviceModelList = await GetPagedAsync(request.Page, request.Limit, deviceModel, cancellationToken);

            return deviceModelList.MapTo<DeviceModelDto>(_mapper);
        }
    }
}