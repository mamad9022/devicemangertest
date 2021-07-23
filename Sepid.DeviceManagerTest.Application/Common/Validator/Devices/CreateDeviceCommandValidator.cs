using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Devices.Command.Create;
using Sepid.DeviceManagerTest.Application.Core.Settings.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Common.Validator.Devices
{
    public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;
        private const string KEY = "BDE6282501A84E0DB9E777D0AE37FED4";
        private const string IV = "2A5658A3A7204E19";

        public CreateDeviceCommandValidator(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;

            CascadeMode = CascadeMode.Stop;
            _localization = localization;

            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired));

            RuleFor(dto => dto.EntranceMode)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.EntranceModeIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.EntranceModeIsRequired))
                .IsInEnum().WithMessage(_localization.GetMessage(ResponseMessage.EntranceModeIsRequired));

            RuleFor(dto => dto.Serial)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.SerialIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.SerialIsRequired))
                .MustAsync(ValidDeviceSerial).WithMessage(_localization.GetMessage(ResponseMessage.DuplicateSerialNumber));

            RuleFor(dto => dto.Ip)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.IpIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.IpIsRequired))
                .MustAsync(ValidIpAddress).WithMessage(_localization.GetMessage(ResponseMessage.IpIsDuplicate));

            RuleFor(dto => dto.SyncLogStartDate)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.SyncLogDateIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.SyncLogDateIsRequired));


            RuleFor(dto => dto.DeviceModelId)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.DeviceModelIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.DeviceModelIsRequired))
                .MustAsync(ValidDeviceModel).WithMessage(_localization.GetMessage(ResponseMessage.DeviceModelNotFound));

            RuleFor(dto => dto).MustAsync(ValidDeviceCount)
                .WithMessage(_localization.GetMessage(ResponseMessage.DeviceCountGreaterThanLicenseRegister))
                .MustAsync(ValidVitalDevice).WithMessage(ResponseMessage.RichVitalDevice);

        }


        public async Task<bool> ValidDeviceSerial(string serial, CancellationToken cancellationToken)
            => !await _context.Devices.AnyAsync(x => x.Serial == serial, cancellationToken);

        public async Task<bool> ValidIpAddress(string ip, CancellationToken cancellationToken)
            => !await _context.Devices.AnyAsync(x => x.Ip == ip, cancellationToken);

        public async Task<bool> ValidDeviceModel(long deviceModelId, CancellationToken cancellationToken)
            => await _context.DeviceModels.AnyAsync(x => x.Id == deviceModelId, cancellationToken);

        private async Task<bool> ValidDeviceCount(CreateDeviceCommand createDeviceCommand, CancellationToken cancellationToken)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(cancellationToken);

            var license = EncryptProvider.AESDecrypt(setting.License, KEY, IV);

            var featureLicense = JsonSerializer.Deserialize<FeatureLicense>(license);

            return (await _context.Devices.CountAsync(cancellationToken) + 1) <= featureLicense.DeviceNumber;
        }

        public async Task<bool> ValidVitalDevice(CreateDeviceCommand createDeviceCommand,
            CancellationToken cancellationToken)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(cancellationToken);
            return await _context.Devices.CountAsync(x => x.IsVital, cancellationToken) <= setting.VitalDevice;
        }
    }
}