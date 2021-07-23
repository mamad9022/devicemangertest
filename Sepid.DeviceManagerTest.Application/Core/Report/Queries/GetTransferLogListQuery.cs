using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Models;

namespace Sepid.DeviceManagerTest.Application.Core.Report.Queries
{
    public class GetTransferLogListQuery : IRequest<List<TransferLog>>
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool? IsSuccess { get; set; }

        public string Query { get; set; }
    }

    public class GetTransferLogListQueryHandler : IRequestHandler<GetTransferLogListQuery, List<TransferLog>>
    {
        private readonly IDeviceManagerContext _context;

        public GetTransferLogListQueryHandler(IDeviceManagerContext context)
        {
            _context = context;
        }

        public async Task<List<TransferLog>> Handle(GetTransferLogListQuery request, CancellationToken cancellationToken)
        {
            request.FromDate ??= new DateTime();
            request.ToDate ??= DateTime.Now;

            IQueryable<TransferLog> transferLogs = _context.TransferLogs
                .Include(x => x.Device)
                .Where(x =>
                    x.CreateDate >= request.FromDate && x.CreateDate <= request.ToDate);

            if (!string.IsNullOrWhiteSpace(request.Query))
                transferLogs = transferLogs.Where(x => x.Description.Contains(request.Query));

            if (request.IsSuccess.HasValue)
                transferLogs = transferLogs.Where(x => x.IsSuccess == request.IsSuccess);

            return await transferLogs.ToListAsync(cancellationToken);
        }
    }
}