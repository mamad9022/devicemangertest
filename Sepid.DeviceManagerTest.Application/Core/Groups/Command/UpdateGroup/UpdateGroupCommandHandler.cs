using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Groups.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Command.UpdateGroup
{
    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, Result>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;
        private readonly IBusPublish _busPublish;
        private readonly IMemoryCache _cache;
        private readonly ILocalization _localization;

        public UpdateGroupCommandHandler(IDeviceManagerContext context, IMapper mapper, IMemoryCache cache, IBusPublish busPublish, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _busPublish = busPublish;
            _localization = localization;
        }

        public async Task<Result> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _context.Groups
                .Include(x => x.DeviceInGroups)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (group is null)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.GroupNotFound,cancellationToken))));


            _mapper.Map(request, group);


            await _context.SaveAsync(cancellationToken);

            var groupViewModel = _mapper.Map<GroupViewModel>(group);

            _busPublish.Send("updateGroup", JsonSerializer.Serialize(groupViewModel));

            _cache.Remove("Groups");

            return Result.SuccessFul();
        }
    }
}