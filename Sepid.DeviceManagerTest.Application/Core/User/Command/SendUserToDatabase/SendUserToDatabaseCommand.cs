using System;
using System.Collections.Generic;
using MediatR;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.User.Command.SendUserToDatabase
{
    public class SendUserToDatabaseCommand : IRequest<Result>
    {
        public int DeviceId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<TemplateDto> Templates { get; set; }
    }
}