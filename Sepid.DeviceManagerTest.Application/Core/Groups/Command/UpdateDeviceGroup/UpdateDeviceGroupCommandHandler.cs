using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Groups.Dto;
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using Sepid.DeviceManagerTest.Common.Results;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Command.UpdateDeviceGroup
{
    public class UpdateDeviceGroupCommandHandler : IRequestHandler<UpdateDeviceGroupCommand, Result>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;
        private readonly IMapper _mapper;
        private readonly IBusPublish _busPublish;

        public UpdateDeviceGroupCommandHandler(IDeviceManagerContext context, ILocalization localization, IMapper mapper, IBusPublish busPublish)
        {
            _context = context;
            _localization = localization;
            _mapper = mapper;
            _busPublish = busPublish;
        }

        public async Task<Result> Handle(UpdateDeviceGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _context.Groups
            .Include(x => x.DeviceInGroups)
            .SingleOrDefaultAsync(x => x.Id == request.GroupId, cancellationToken);

            if (group is null)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.GroupNotFound, cancellationToken))));

            _context.DeviceInGroups.RemoveRange(group.DeviceInGroups);
            List<DeviceInGroup> deviceInGroups = new List<DeviceInGroup>();

            foreach (var deviceId in request.DeviceIds)
            {
                if (!await _context.Devices.AnyAsync(x => x.Id == deviceId, cancellationToken))
                    return Result.Failed(new BadRequestObjectResult(await _localization.GetMessage(ResponseMessage.DeviceNotFound, cancellationToken)));

                deviceInGroups.Add(new DeviceInGroup
                {
                    DeviceId = deviceId,
                    Group = group
                });
            }

            await _context.DeviceInGroups.AddRangeAsync(deviceInGroups, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            var groupViewModel = _mapper.Map<GroupViewModel>(group);

            groupViewModel.DeviceIds = deviceInGroups.Select(x => x.DeviceId).ToList();

            _busPublish.Send("updateGroup", JsonSerializer.Serialize(groupViewModel));


            return Result.SuccessFul();
        }
    }
}
