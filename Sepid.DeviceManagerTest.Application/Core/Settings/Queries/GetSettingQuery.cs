using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Settings.Dto;
using Sepid.DeviceManagerTest.Common.Results;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Core.Settings.Queries
{
    public class GetSettingQuery : IRequest<Result<SettingDto>>
    {

    }


    public class GetSettingQueryHandler : IRequestHandler<GetSettingQuery, Result<SettingDto>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;

        public GetSettingQueryHandler(IDeviceManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<SettingDto>> Handle(GetSettingQuery request, CancellationToken cancellationToken)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(cancellationToken);

            var appSetting = _mapper.Map<SettingDto>(setting);

            GetSystemId(appSetting);

            appSetting.VitalDeviceUse = await _context.Devices.CountAsync(x => x.IsVital, cancellationToken);

            return Result<SettingDto>.SuccessFul(appSetting);
        }

        private static void GetSystemId(SettingDto appSetting)
        {
            try
            {
                var path = Path.Combine("Resources", "activation.txt");

                var systemId = File.ReadLines(path).First().Replace("SystemId:", "");

                appSetting.SystemId = systemId;
            }
            catch (global::System.Exception)
            {

                appSetting.SystemId = "";
            }

        }
    }
}