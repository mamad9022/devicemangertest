using FluentValidation;
using Sepid.DeviceManagerTest.Application.Core.Settings.Command.Update;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Common.Validator.Settings
{
    public class UpdateSettingCommandValidator : AbstractValidator<UpdateSettingCommand>
    {
        private readonly ILocalization _localization;
        public UpdateSettingCommandValidator(ILocalization localization)
        {
            _localization = localization;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.RetryFailedTransferNumber)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.RetryNumberRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.RetryNumberRequired))
                .GreaterThan(1).WithMessage(_localization.GetMessage(ResponseMessage.RetryNumberMustGreteaherThanOne));


            RuleFor(dto => dto.FingerPrintQuality)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.FingerPrintQualityRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.FingerPrintQualityRequired))
                .GreaterThan(10).WithMessage(_localization.GetMessage(ResponseMessage.FingerPrintQualityRang))
                .LessThanOrEqualTo(100).WithMessage(_localization.GetMessage(ResponseMessage.FingerPrintQualityRang));


            RuleFor(dto => dto.VitalDevice).NotEmpty()
                .NotNull().GreaterThanOrEqualTo(1)
                .WithMessage(_localization.GetMessage(ResponseMessage.MinimumVitalDevice))
                .LessThanOrEqualTo(40).WithMessage(_localization.GetMessage(ResponseMessage.MaximumVitalDevice));
        }
    }
}