using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Groups.Command.CreateGroup;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Common.Validator.Groups
{
    public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;
        public CreateGroupCommandValidator(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            CascadeMode = CascadeMode.Stop;
            _localization = localization;

            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
                .MustAsync(ValidName).WithMessage(_localization.GetMessage(ResponseMessage.NameAlreadyExist));

            RuleFor(dto => dto.ParentId).MustAsync(ValidGroup).WithMessage(_localization.GetMessage(ResponseMessage.GroupNotFound));

        }

        private async Task<bool> ValidName(string name, CancellationToken cancellation)
        {
            if (await _context.Groups.AnyAsync(x => x.Name == name, cancellation))
                return false;

            return true;
        }

        private async Task<bool> ValidGroup(long? parentId, CancellationToken cancellationToken)
        {
            if (parentId.HasValue)
                if (await _context.Groups.AnyAsync(x => x.Id == parentId, cancellationToken) == false)
                    return false;

            return true;
        }


    }
}