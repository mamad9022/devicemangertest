using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Devices.Command.Update;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Common.Validator.Devices
{
    public class UpdateDeviceCommandValidator : AbstractValidator<UpdateDeviceCommand>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;

        public UpdateDeviceCommandValidator(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Id)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.IdIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.IdIsRequired));

            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired));

            RuleFor(dto => dto.Serial)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.SerialIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.SerialIsRequired));

            RuleFor(dto => dto.Ip)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.IpIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.IpIsRequired));

            RuleFor(dto => dto.SyncLogStartDate)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.SyncLogDateIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.SyncLogDateIsRequired));


            RuleFor(dto => dto.DeviceModelId)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.DeviceModelIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.DeviceModelIsRequired))
                .MustAsync(ValidDeviceModel).WithMessage(_localization.GetMessage(ResponseMessage.DeviceModelNotFound));

            RuleFor(dto => dto)
                .MustAsync(ValidDeviceSerial).WithMessage(_localization.GetMessage(ResponseMessage.DuplicateSerialNumber))
                .MustAsync(ValidIpAddress).WithMessage(_localization.GetMessage(ResponseMessage.IpIsDuplicate));

            RuleFor(dto => dto.EntranceMode)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.EntranceModeIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.EntranceModeIsRequired))
                .IsInEnum().WithMessage(_localization.GetMessage(ResponseMessage.EntranceModeIsRequired)); ;

        }

        public bool ValidSyncLogPeriodTime(string syncLogPeriod) => TimeSpan.TryParse(syncLogPeriod, out _);

        public async Task<bool> ValidDeviceSerial(UpdateDeviceCommand updateDeviceCommand, CancellationToken cancellationToken)
            => !await _context.Devices.AnyAsync(x => x.Serial == updateDeviceCommand.Serial && x.Id != updateDeviceCommand.Id, cancellationToken);

        public async Task<bool> ValidIpAddress(UpdateDeviceCommand updateDeviceCommand, CancellationToken cancellationToken)
            => !await _context.Devices.AnyAsync(x => x.Ip == updateDeviceCommand.Ip && x.Id != updateDeviceCommand.Id, cancellationToken);

        public async Task<bool> ValidDeviceModel(long deviceModelId, CancellationToken cancellationToken)
            => await _context.DeviceModels.AnyAsync(x => x.Id == deviceModelId, cancellationToken);


        public async Task<bool> ValidVitalDevice(UpdateDeviceCommand updateDeviceCommand,
            CancellationToken cancellation)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(cancellation);

            return await _context.Devices.CountAsync(x=>x.IsVital && x.Id!=updateDeviceCommand.Id, cancellationToken: cancellation) <= setting.VitalDevice;
        }
    }
}