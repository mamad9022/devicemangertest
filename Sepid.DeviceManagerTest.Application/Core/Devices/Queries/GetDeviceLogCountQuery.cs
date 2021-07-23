using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sepid.DeviceManagerTest.Common.Results;
using NotImplementedException = System.NotImplementedException;

namespace Sepid.DeviceManagerTest.Application.Core.Devices.Queries
{
    public class GetDeviceLogCountQuery : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class GetDeviceLogCountQueryHandler : IRequestHandler<GetDeviceLogCountQuery,Result<int>>
    {

        public Task<Result<int>> Handle(GetDeviceLogCountQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}