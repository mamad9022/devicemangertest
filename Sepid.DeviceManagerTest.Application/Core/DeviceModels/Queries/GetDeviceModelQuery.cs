using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Results;
using System.Threading;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceModels.Queries
{
    public class GetDeviceModelQuery : IRequest<Result<DeviceModelDto>>
    {
        public int Id { get; set; }
    }

    public class GetDeviceModelQueryHandler : IRequestHandler<GetDeviceModelQuery, Result<DeviceModelDto>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public GetDeviceModelQueryHandler(IDeviceManagerContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result<DeviceModelDto>> Handle(GetDeviceModelQuery request, CancellationToken cancellationToken)
        {
            var deviceModel =
                await _context.DeviceModels.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (deviceModel is null)
                return Result<DeviceModelDto>.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceModelNotFound,cancellationToken))));

            return Result<DeviceModelDto>.SuccessFul(_mapper.Map<DeviceModelDto>(deviceModel));
        }
    }
}