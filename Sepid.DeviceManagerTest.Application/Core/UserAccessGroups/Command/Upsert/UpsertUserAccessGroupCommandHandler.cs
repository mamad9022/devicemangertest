using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Models;
using Serilog;

namespace Sepid.DeviceManagerTest.Application.Core.UserAccessGroups.Command.Upsert
{
    public class UpsertUserAccessGroupCommandHandler : IRequestHandler<UpsertUserAccessGroupCommand, Unit>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;


        public UpsertUserAccessGroupCommandHandler(IDeviceManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpsertUserAccessGroupCommand request, CancellationToken cancellationToken)
        {
            var userGroup =
                await _context.UserAccessGroups.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

            try
            {
                //create new userAccessGroup
                if (userGroup is null)
                {
                    var createUserAccessGroup = _mapper.Map<UserAccessGroup>(request);
                    await _context.UserAccessGroups.AddAsync(createUserAccessGroup, cancellationToken);
                }
                //Update userAccessGroup
                else
                {
                    _mapper.Map(request, userGroup);
                    userGroup.GroupIds = request.GroupIds ??= new List<long>();
                }

                await _context.SaveAsync(cancellationToken);

            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.StackTrace);
            }

            return Unit.Value;

        }
    }
}