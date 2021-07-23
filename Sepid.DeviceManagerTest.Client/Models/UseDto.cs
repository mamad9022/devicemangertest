using System;
using System.Collections.Generic;
using Sepid.DeviceManagerTest.Client.Enums;

namespace Sepid.DeviceManagerTest.Client.Models
{
    public class UseDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public OperatorLevel OperatorLevel { get; set; } = OperatorLevel.None;
        public CardAuthMode CardAuthMode { get; set; } = CardAuthMode.None;
        public FingerAuthMode FingerAuthMode { get; set; } = FingerAuthMode.None;
        public FaceAuthMode FaceAuthMode { get; set; } = FaceAuthMode.None;
        public SecurityLevel SecurityLevel { get; set; } = SecurityLevel.Normal;
        public AuthMode AuthMode { get; set; }

        public List<TemplateDto> Templates { get; set; }
    }
    
    
}