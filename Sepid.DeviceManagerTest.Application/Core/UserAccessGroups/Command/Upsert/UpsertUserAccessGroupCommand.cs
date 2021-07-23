using System.Collections.Generic;
using MediatR;

namespace Sepid.DeviceManagerTest.Application.Core.UserAccessGroups.Command.Upsert
{
    public class UpsertUserAccessGroupCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public List<long> GroupIds { get; set; }
    }
}