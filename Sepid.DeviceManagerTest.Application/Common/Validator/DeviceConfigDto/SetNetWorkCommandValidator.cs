using FluentValidation;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.SetNetworkCommand;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Common.Enum;

namespace Sepid.DeviceManagerTest.Application.Common.Validator.DeviceConfigDto
{
    public class SetNetWorkCommandValidator : AbstractValidator<SetNetWorkCommand>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;

        public SetNetWorkCommandValidator(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto)
                .Must(ValidIpAddress).WithMessage(_localization.GetMessage(ResponseMessage.CanNotSetIpAddress))
                .Must(ValidGatewayAddress).WithMessage(_localization.GetMessage(ResponseMessage.InvalidGateway))
                .Must(ValidSubnetMaskAddress).WithMessage(_localization.GetMessage(ResponseMessage.InvalidSubnetMask))
                .Must(ValidServerAddress).WithMessage(_localization.GetMessage(ResponseMessage.InvalidServerIpAddress))
                .Must(IsIpAllocated).WithMessage(_localization.GetMessage(ResponseMessage.IpAllocatedInNetwork))
                .MustAsync(ValidServerMatching).WithMessage(_localization.GetMessage(ResponseMessage.MatchOnServerSupportV2));

        }


        private async Task<bool> ValidServerMatching(SetNetWorkCommand setNetWorkCommand,CancellationToken cancellationToken)
        {
            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == setNetWorkCommand.DeviceId, cancellationToken);
            if (setNetWorkCommand.IsMatchOnServer && device.DeviceModel.SdkType == SdkType.ZkTechno)
            {
                return false;
            }

            return true;

        }

        private bool ValidIpAddress(SetNetWorkCommand setNetWork)
        {
            if (setNetWork.UseDhcp == false)
                if (IPAddress.TryParse(setNetWork.Ip, out _) == false)
                    return false;

            return true;
        }

        private bool ValidGatewayAddress(SetNetWorkCommand setNetWork)
        {
            if (setNetWork.UseDhcp == false)
                if (IPAddress.TryParse(setNetWork.GateWay, out _) == false)
                    return false;

            return true;
        }

        private bool ValidSubnetMaskAddress(SetNetWorkCommand setNetWork)
        {
            if (setNetWork.UseDhcp == false)
                if (IPAddress.TryParse(setNetWork.SubnetMask, out _) == false)
                    return false;

            return true;
        }

        private bool ValidServerAddress(SetNetWorkCommand setNetWorkCommand)
        {
            if (setNetWorkCommand.IsDeviceToServer)
                if (IPAddress.TryParse(setNetWorkCommand.ServerAddress, out _) == false)
                    return false;

            return true;
        }

        private bool IsIpAllocated(SetNetWorkCommand setNetworkCommand)
        {
            bool pingAble = false;
            Ping ping = null;
            if (_context.Devices.Any(x => x.Id == setNetworkCommand.DeviceId && x.Ip != setNetworkCommand.Ip) )
            {
                try
                {
                    ping = new Ping();
                    PingReply reply = ping.Send(setNetworkCommand.Ip);
                    if (reply != null) pingAble = reply.Status != IPStatus.Success;
                }
                catch (PingException)
                {
                    return true;
                    // Discard PingExceptions and return false;
                }
                finally
                {
                    ping?.Dispose();
                }
                return pingAble;
            }

            return true;
        }
    }
}