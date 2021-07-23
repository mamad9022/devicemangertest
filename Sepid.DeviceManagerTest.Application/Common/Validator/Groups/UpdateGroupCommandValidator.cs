using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Groups.Command.UpdateGroup;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Common.Validator.Groups
{
    public class UpdateGroupCommandValidator : AbstractValidator<UpdateGroupCommand>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;
        public UpdateGroupCommandValidator(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            CascadeMode = CascadeMode.Stop;
            _localization = localization;
            RuleFor(dto => dto.Id).NotEmpty().NotNull();

            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired));

            RuleFor(dto => dto.ParentId)
                .MustAsync(ValidGroup).WithMessage(_localization.GetMessage(ResponseMessage.GroupNotFound));

            RuleFor(dto => dto).MustAsync(ValidName).WithMessage(_localization.GetMessage(ResponseMessage.NameAlreadyExist));

        }

        private async Task<bool> ValidName(UpdateGroupCommand updateGroup, CancellationToken cancellationToken)
        {
            if (await _context.Groups.AnyAsync(x => x.Name == updateGroup.Name && x.Id != updateGroup.Id, cancellationToken))
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