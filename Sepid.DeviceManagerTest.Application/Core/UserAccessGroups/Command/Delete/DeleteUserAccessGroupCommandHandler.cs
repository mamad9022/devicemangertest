using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.UserAccessGroups.Command.Delete;
using Serilog;

namespace Sepid.DeviceManager.Application.Core.UserAccessGroups.Command.Delete
{
    public class DeleteUserAccessGroupCommandHandler : IRequestHandler<DeleteUserAccessGroupCommand, Unit>
    {
        private readonly IDeviceManagerContext _context;

        public DeleteUserAccessGroupCommandHandler(IDeviceManagerContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUserAccessGroupCommand request, CancellationToken cancellationToken)
        {
            var userAccessGroup =
                await _context.UserAccessGroups.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            try
            {
                if (userAccessGroup != null)
                {
                    userAccessGroup.IsDeleted = true;
                    await _context.SaveAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.StackTrace);
            }

            return Unit.Value;


        }
    }
}