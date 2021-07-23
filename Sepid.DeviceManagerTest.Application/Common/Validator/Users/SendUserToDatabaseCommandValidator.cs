using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.User.Command.SendUserToDatabase;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Common.Validator.Users
{
    public class SendUserToDatabaseCommandValidator : AbstractValidator<SendUserToDatabaseCommand>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;
        public SendUserToDatabaseCommandValidator(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired));

            RuleFor(dto => dto.Code)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.CodeIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.CodeIsRequired));

            RuleFor(dto => dto.StartDate)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.StartDataIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.StartDataIsRequired));

            RuleFor(dto => dto.EndDate)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.EndDateIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.EndDateIsRequired));

            RuleFor(dto => dto.DeviceId).NotEmpty().NotNull()
                .MustAsync(ValidDevice).WithMessage(_localization.GetMessage(ResponseMessage.DeviceNotFound));

        }

        private async Task<bool> ValidDevice(int deviceId, CancellationToken cancellationToken)
        {
            if (await _context.Devices.AnyAsync(x => x.Id == deviceId, cancellationToken))
                return true;

            return false;
        }

    }
}