using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Devices.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Results;
using System.Threading;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Core.Devices.Queries
{
    public class GetDeviceQuery : IRequest<Result<DeviceDto>>
    {
        public int Id { get; set; }
    }

    public class GetDeviceQueryHandler : IRequestHandler<GetDeviceQuery, Result<DeviceDto>>
    {
        private readonly IMapper _mapper;
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;

        public GetDeviceQueryHandler(IMapper mapper, IDeviceManagerContext context, ILocalization localization)
        {
            _mapper = mapper;
            _context = context;
            _localization = localization;
        }

        public async Task<Result<DeviceDto>> Handle(GetDeviceQuery request, CancellationToken cancellationToken)
        {
            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (device is null)
                return Result<DeviceDto>.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound,cancellationToken))));

            return Result<DeviceDto>.SuccessFul(_mapper.Map<DeviceDto>(device));
        }
    }
}