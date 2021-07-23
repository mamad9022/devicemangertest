using Sepid.DeviceManagerTest.Client.Enums;
using System;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Client.Models
{
    public class EnrollUserCommand
    {
        public string DeviceSerial { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public OperatorLevel OperatorLevel { get; set; } = OperatorLevel.None;
        public CardAuthMode? CardAuthMode { get; set; } = Enums.CardAuthMode.None;
        public FingerAuthMode? FingerAuthMode { get; set; } = Enums.FingerAuthMode.None;
        public FaceAuthMode? FaceAuthMode { get; set; } = Enums.FaceAuthMode.None;
        public SecurityLevel SecurityLevel { get; set; } = SecurityLevel.Normal;
        public AuthMode AuthMode { get; set; } = AuthMode.Disable;
        public List<TemplateDto> Templates { get; set; }
    }

    public class TemplateDto
    {
        public string TemplateData { get; set; }
        public TemplateType TemplateType { get; set; }
        public FingerIndex FingerIndex { get; set; }
        public string Image { get; set; }
    }
}