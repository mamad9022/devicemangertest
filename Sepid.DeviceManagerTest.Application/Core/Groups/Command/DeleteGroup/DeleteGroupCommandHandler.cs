using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Command.DeleteGroup
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Result>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMemoryCache _cache;
        private readonly IBusPublish _busPublish;
        private readonly ILocalization _localization;
        private readonly IGroupService _groupService;

        public DeleteGroupCommandHandler(IDeviceManagerContext context, IMemoryCache cache, IBusPublish busPublish, ILocalization localization, IGroupService groupService)
        {
            _context = context;
            _cache = cache;
            _busPublish = busPublish;
            _localization = localization;
            _groupService = groupService;
        }

        public async Task<Result> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _context.Groups
                .Include(x=>x.Children)
                .ThenInclude(x=>x.Children)
                .ThenInclude(x=>x.Children)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (group is null)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.GroupNotFound,cancellationToken))));

            group.IsDeleted = true;

            if (group.Children.Any())
            {
                _groupService.DeleteAllGroupWithChild(group.Children.ToList());
            }

            _busPublish.Send("deleteGroup", JsonSerializer.Serialize(new { id = group.Id }));

            await _context.SaveAsync(cancellationToken);

            _cache.Remove("Groups");

            return Result.SuccessFul();
        }
    }
}