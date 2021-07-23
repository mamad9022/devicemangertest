using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sepid.DeviceManagerTest.Common.Options;

namespace Sepid.DeviceManagerTest.Application.Core.Settings.Queries
{
    public class GetSystemVersionQuery : IRequest<string>
    {
        
    }

    public class GetSystemVersionQueryHandler: IRequestHandler<GetSystemVersionQuery,string>
    {
        private readonly IOptionsMonitor<SystemSetting> _systemSetting;

        public GetSystemVersionQueryHandler(IOptionsMonitor<SystemSetting> systemSetting)
        {
            _systemSetting = systemSetting;
        }

        public Task<string> Handle(GetSystemVersionQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_systemSetting.CurrentValue.Version);
        }
    }
}