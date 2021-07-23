using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Sepid.DeviceManagerTest.Common.Helper;

namespace Sepid.DeviceManagerTest.Application.Common.Interfaces
{
    public interface IUserAccessGroupService
    {
        Task<List<int>> GetAllDeviceAccessId();
    }

    public class UserAccessGroupService : IUserAccessGroupService
    {
        private readonly IDeviceManagerContext _context;
        private readonly IDistributedCache _cache;
        //private readonly ICurrentUserService _currentUserService;

        public UserAccessGroupService(IDistributedCache cache,
            IDeviceManagerContext context)
        {
            //_currentUserService = currentUserService;
            _cache = cache;
            _context = context;
        }

        public async Task<List<int>> GetAllDeviceAccessId()
        {
            //var deviceIds = await _cache.GetRecordAsync<List<int>>($"UserDeviceAccess-{_currentUserService.UserId}",
            //    CancellationToken.None);

            //if (deviceIds is null)
            //{
                //var userAccessGroup =
                //    await _context.UserAccessGroups
                //        .FirstOrDefaultAsync(x => x.UserId == int.Parse(_currentUserService.UserId));

                //if (userAccessGroup is null) return new List<int>();

                //if (userAccessGroup.GroupIds.Count == 0) return new List<int>();

                //var groups = await _context.Groups
                //    .Where(x => userAccessGroup.GroupIds.Contains(x.Id))
                //    .Include(x => x.Children)
                //    .ThenInclude(x => x.Children)
                //    .ToListAsync();

                //List<long> groupIds = new List<long>();
                //groups.ForEach(group =>
                //{
                //    groupIds.Add(group.Id);

                //    if (group.Children.Any())
                //    {
                //        groupIds.AddRange(group.Children.Select(x => x.Id));
                //    }

                //    if (group.Children.Any(g => g.Children.Any()))
                //    {
                //        groupIds.AddRange(group.Children.Where(x => x.Children.Count > 0).Select(x => x.Id).ToList());
                //    }
                //});
            List<int> deviceIds = new List<int>();
                //deviceIds = await _context.DeviceInGroups
                //    .Where(x => groupIds.Contains(x.GroupId))
                //    .Select(x => x.DeviceId).ToListAsync();

                //await _cache.SetRecordAsync($"UserDeviceAccess-{_currentUserService.UserId}", deviceIds,
                //    CancellationToken.None);
         //   }

            return deviceIds;
        }
    }
}