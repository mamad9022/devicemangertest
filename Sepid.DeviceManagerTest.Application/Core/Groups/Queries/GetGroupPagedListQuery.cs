using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Pagination;
using Sepid.DeviceManagerTest.Application.Core.Groups.Dto;
using Sepid.DeviceManagerTest.Application.Models;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Queries
{
    public class GetGroupPagedListQuery : PagingOptions, IRequest<PagedList<GroupDto>>
    {
    }

    public class GetGroupPagedListQueryHandler : PagingService<Group>, IRequestHandler<GetGroupPagedListQuery, PagedList<GroupDto>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;

        public GetGroupPagedListQueryHandler(IDeviceManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<GroupDto>> Handle(GetGroupPagedListQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Group> groups = _context.Groups.Include(x => x.DeviceInGroups);

            if (!string.IsNullOrWhiteSpace(request.Query))
                groups = groups.Where(x => x.Name.Contains(request.Query));

            var groupPagedList = await GetPagedAsync(request.Page, request.Limit, groups, cancellationToken);

            return groupPagedList.MapTo<GroupDto>(_mapper);
        }
    }
}