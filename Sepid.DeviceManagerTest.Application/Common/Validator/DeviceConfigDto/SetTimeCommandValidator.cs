using FluentValidation;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.SetTimeCommand;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Common.Validator.DeviceConfigDto
{
    public class SetTimeCommandValidator : AbstractValidator<SetTimeCommand>
    {
        private readonly ILocalization _localization;
        public SetTimeCommandValidator(ILocalization localization)
        {
            _localization = localization;
            RuleFor(dto => dto.Date)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.DateIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.DateIsRequired));

        }
    }
}