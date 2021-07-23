using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.User.Command.DeleteAllUser
{
    public class DeleteAllUserCommand : IRequest<Result>
    {
        public int DeviceId { get; set; }
    }
}