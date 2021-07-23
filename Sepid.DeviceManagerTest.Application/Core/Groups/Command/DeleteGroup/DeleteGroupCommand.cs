using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Command.DeleteGroup
{
    public class DeleteGroupCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}