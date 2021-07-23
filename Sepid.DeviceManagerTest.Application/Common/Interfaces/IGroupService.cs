using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.RabbitMq;

namespace Sepid.DeviceManagerTest.Application.Common.Interfaces
{
    public interface IGroupService
    {
        void DeleteAllGroupWithChild(List<Group> groups);
    }

    public class GroupService : IGroupService
    {
        private readonly IBusPublish _busPublish;

        public GroupService(IBusPublish busPublish)
        {
            _busPublish = busPublish;
        }

        public void DeleteAllGroupWithChild(List<Group> groups)
        {
            foreach (var group in groups)
            {
                group.IsDeleted = true;

                _busPublish.Send("deleteGroup", JsonSerializer.Serialize(new { id = group.Id }));

                if (group.Children.Any())
                {
                    DeleteAllGroupWithChild(group.Children.ToList());
                }
            }
        }
    }
}