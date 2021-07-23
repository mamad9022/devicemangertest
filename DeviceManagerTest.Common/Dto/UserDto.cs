using Sepid.DeviceManagerTest.Common.Enum;
using System;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Common.Dto
{
    public class UserDto
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

    public class TemplateDto
    {
        public string TemplateData { get; set; }
        public byte[] Template { get; set; }
        public TemplateType TemplateType { get; set; }
        public FingerIndex FingerIndex { get; set; }
        public string Image { get; set; }
    }
}