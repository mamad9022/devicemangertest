using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Groups.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Queries
{
    public class GetGroupQuery : IRequest<Result<GroupDto>>
    {
        public int Id { get; set; }
    }

    public class GetGroupQueryHandler : IRequestHandler<GetGroupQuery, Result<GroupDto>>
    {
        private readonly IDeviceManagerContext _context;

        private readonly IMapper _mapper;

        private readonly ILocalization _localization;

        public GetGroupQueryHandler(IDeviceManagerContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result<GroupDto>> Handle(GetGroupQuery request, CancellationToken cancellationToken)
        {
            var group = await _context.Groups
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (group is null)
                return Result<GroupDto>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.GroupNotFound, cancellationToken))));

            return Result<GroupDto>.SuccessFul(_mapper.Map<GroupDto>(group));
        }
    }
}