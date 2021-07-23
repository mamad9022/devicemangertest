using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Results;
using System.Threading;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceModels.Command.Update
{
    public class UpdateDeviceModelCommandHandler : IRequestHandler<UpdateDeviceModelCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IDeviceManagerContext _context;
        private readonly IDistributedCache _cache;
        private readonly ILocalization _localization;

        public UpdateDeviceModelCommandHandler(IMapper mapper, IDeviceManagerContext context, IDistributedCache cache, ILocalization localization)
        {
            _mapper = mapper;
            _context = context;
            _cache = cache;
            _localization = localization;
        }

        public async Task<Result> Handle(UpdateDeviceModelCommand request, CancellationToken cancellationToken)
        {
            var deviceModel =
                await _context.DeviceModels.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (deviceModel is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceModelNotFound,cancellationToken))));

            _mapper.Map(request, deviceModel);

            await _cache.RemoveAsync("getAllDeviceModels", cancellationToken);

            return Result.SuccessFul();
        }
    }
}