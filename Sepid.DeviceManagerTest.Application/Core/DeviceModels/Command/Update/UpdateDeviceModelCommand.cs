using MediatR;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceModels.Command.Update
{
    public class UpdateDeviceModelCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Code { get; set; }
        public SdkType SdkType { get; set; }
        public bool IsFingerSupport { get; set; }
        public bool IsFaceSupport { get; set; }
        public bool IsCardSupport { get; set; }
        public bool IsPasswordSupport { get; set; }
    }
}