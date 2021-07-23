using MediatR;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.Results;
using System;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Core.User.Command.EnrollUser
{
    public class EnrollUserCommand : IRequest<Result>
    {
        public string DeviceSerial { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public OperatorLevel OperatorLevel { get; set; } = OperatorLevel.None;
        public CardAuthMode? CardAuthMode { get; set; } = DeviceManagerTest.Common.Enum.CardAuthMode.None;
        public FingerAuthMode? FingerAuthMode { get; set; } = DeviceManagerTest.Common.Enum.FingerAuthMode.None;
        public FaceAuthMode? FaceAuthMode { get; set; } = DeviceManagerTest.Common.Enum.FaceAuthMode.None;
        public SecurityLevel SecurityLevel { get; set; } = SecurityLevel.Normal;
        public AuthMode AuthMode { get; set; } = AuthMode.Disable;
        public List<TemplateDto> Templates { get; set; }
    }
}