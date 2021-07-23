using System;
using System.Collections.Generic;
using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.BatchOperation.SetTime
{
    public class SetTimeOperationCommand : IRequest<Result>
    {
        public List<long> GroupIds { get; set; }

        public DateTime Date { get; set; }
    }
}