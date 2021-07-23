using FluentValidation;
using Sepid.DeviceManagerTest.Application.Core.Auth.Command;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Common.Validator.AuthConfigs
{
    public class SetAuthConfigCommandValidator : AbstractValidator<SetAuthConfigCommand>
    {
        private readonly ILocalization _localization;
        public SetAuthConfigCommandValidator(ILocalization localization)
        {
            CascadeMode = CascadeMode.Stop;
            _localization = localization;

            RuleFor(dto => dto.AuthTimeout)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.AuthTimeOutIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.AuthTimeOutIsRequired))
                .GreaterThanOrEqualTo(10).WithMessage(_localization.GetMessage(ResponseMessage.AuthTimeOutGreaterThanTenSecond));

            RuleFor(dto => dto.MatchTimeout)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.MathTimeOutIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.MathTimeOutIsRequired))
                .GreaterThanOrEqualTo(5).WithMessage(_localization.GetMessage(ResponseMessage.MatchTimeOutGreaterThanFiveSecond));

            RuleFor(dto => dto.AuthConfigIds)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.AuthConfigIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.AuthConfigIsRequired));


            RuleForEach(dto => dto.AuthConfigIds)
                .IsInEnum().WithMessage(_localization.GetMessage(ResponseMessage.NotValidAuthConfigType));

        }





    }
}