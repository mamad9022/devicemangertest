﻿using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.BatchOperation.UnlockDevice;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Common.Validator.BatchOperations
{
    public class UnlockDeviceCommandValidator : AbstractValidator<UnlockDeviceOperationCommand>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;

        public UnlockDeviceCommandValidator(ILocalization localization, IDeviceManagerContext context)
        {
            _localization = localization;
            _context = context;
            CascadeMode = CascadeMode.Stop;


            RuleForEach(dto => dto.GroupIds)
                .NotEmpty().NotNull()
                .MustAsync(ValidGroupId)
                .WithMessage(_localization.GetMessage(ResponseMessage.GroupNotFound));
        }
        private async Task<bool> ValidGroupId(long id, CancellationToken cancellationToken)
        {
            if (await _context.Groups.AnyAsync(x => x.Id == id, cancellationToken) == false)
            {
                return false;
            }

            return true;
        }
    }
}