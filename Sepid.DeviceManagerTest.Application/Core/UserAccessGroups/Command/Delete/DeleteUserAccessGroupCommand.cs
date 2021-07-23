using MediatR;

namespace Sepid.DeviceManagerTest.Application.Core.UserAccessGroups.Command.Delete
{
    public class DeleteUserAccessGroupCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}