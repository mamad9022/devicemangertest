using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Groups.Dto;
using Sepid.DeviceManagerTest.Application.Models;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Queries
{
    public class GetGroupListQuery : IRequest<List<GroupDto>>
    {
    }

    public class GetGroupListQueryHandler : IRequestHandler<GetGroupListQuery, List<GroupDto>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public GetGroupListQueryHandler(IDeviceManagerContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<GroupDto>> Handle(GetGroupListQuery request, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue("Groups", out List<Group> groups))
            {
                groups = await _context.Groups.Where(x => x.ParentId == null)
                    .OrderBy(x => x.Name)
                    .Include(x => x.Children)
                    .ThenInclude(x => x.Children)
                    .ThenInclude(x => x.Children)
                    .ToListAsync(cancellationToken);

                _cache.Set("Groups", groups,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
            }

            return _mapper.Map<List<GroupDto>>(groups);
        }
    }
}