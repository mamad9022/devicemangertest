using System.Collections.Generic;
using System.Linq;
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
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Command.CreateGroup
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Result<GroupDto>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IBusPublish _busPublish;

        public CreateGroupCommandHandler(IDeviceManagerContext context, IMapper mapper, IMemoryCache cache, IBusPublish busPublish)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _busPublish = busPublish;
        }

        public async Task<Result<GroupDto>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = _mapper.Map<Group>(request);

            await _context.Groups.AddAsync(group, cancellationToken);

            List<DeviceInGroup> deviceInGroups = new List<DeviceInGroup>();

            if (request.DeviceIds!=null)
            {
                foreach (var deviceId in request.DeviceIds)
                {
                    if (!await _context.Devices.AnyAsync(x => x.Id == deviceId, cancellationToken))
                        return Result<GroupDto>.Failed(new BadRequestObjectResult(ResponseMessage.DeviceNotFound));

                    deviceInGroups.Add(new DeviceInGroup
                    {
                        DeviceId = deviceId,
                        Group = group
                    });
                }
            }
 

            await _context.SaveAsync(cancellationToken);

            await _context.DeviceInGroups.AddRangeAsync(deviceInGroups, cancellationToken);

            var groupViewModel = _mapper.Map<GroupViewModel>(group);

            groupViewModel.DeviceIds = deviceInGroups.Select(x => x.DeviceId).ToList();

            _busPublish.Send("updateGroup", JsonSerializer.Serialize(groupViewModel));

            _cache.Remove("Groups");

            return Result<GroupDto>.SuccessFul(_mapper.Map<GroupDto>(group));
        }
    }
}