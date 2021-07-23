using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Pagination;
using Sepid.DeviceManagerTest.Application.Core.Report.Dto;
using Sepid.DeviceManagerTest.Application.Models;

namespace Sepid.DeviceManagerTest.Application.Core.Report.Queries
{
    public class GetTransferLogPagedListQuery : PagingOptions, IRequest<PagedList<TransferLogDto>>
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool? IsSuccess { get; set; }
    }


    public class GetTransferLogPagedListQueryHandler : PagingService<TransferLog>,
        IRequestHandler<GetTransferLogPagedListQuery, PagedList<TransferLogDto>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;

        public GetTransferLogPagedListQueryHandler(IDeviceManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<TransferLogDto>> Handle(GetTransferLogPagedListQuery request,
            CancellationToken cancellationToken)
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

            var transferLogList = await GetPagedAsync(request.Page, request.Limit,
                transferLogs.OrderByDescending(x => x.CreateDate), cancellationToken);

            return transferLogList.MapTo<TransferLogDto>(_mapper);
        }
    }
}