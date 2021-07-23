using FluentValidation;
using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Command.Update;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Common.Validator.DeviceModels
{
    public class UpdateDeviceModelCommandValidator : AbstractValidator<UpdateDeviceModelCommand>
    {
        private readonly ILocalization _localization;
        public UpdateDeviceModelCommandValidator(ILocalization localization)
        {
            _localization = localization;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Id)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.IdIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.IdIsRequired));

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