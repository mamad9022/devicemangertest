using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.User.Command.DeleteUser
{
    public class DeleteUserCommand : IRequest<Result>
    {
        public int DeviceId { get; set; }
      
        public string PersonCode { get; set; }

        public string Name { get; set; }
    }
}