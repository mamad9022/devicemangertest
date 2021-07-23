using FluentValidation;
using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Command.Create;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Common.Validator.DeviceModels
{
    public class CreateDeviceModelCommandValidator : AbstractValidator<CreateDeviceModelCommand>
    {
        private readonly ILocalization _localization;
        public CreateDeviceModelCommandValidator(ILocalization localization)
        {
            _localization = localization;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired));

            RuleFor(dto => dto.SdkType)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.SdkTypeIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.SdkTypeIsRequired));

            RuleFor(dto => dto.Code)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.CodeIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.CodeIsRequired));

        }
    }
}