using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.Enum;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Core.System
{
    public class SeedData
    {
        private readonly IDeviceManagerContext _context;

        public SeedData(IDeviceManagerContext context)
        {
            _context = context;
        }

        public async Task SeedAllAsync(CancellationToken cancellationToken)
        {
            if (!await _context.DeviceModels.AnyAsync(cancellationToken))
            {
                await _context.DeviceModels.AddAsync(new DeviceModel
                {
                    Name = "UFace202",
                    Code = DeviceModelTypes.uFace202,
                    IsCardSupport = true,
                    IsFingerSupport = false,
                    IsFaceSupport = true,
                    IsPasswordSupport = true,
                    SdkType = SdkType.ZkTechno,
                    Image = "/Files/Image/FaceStation2.png",
                    TotalLog = 5000000
                }, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "BioStationA2",
                //    Code = DeviceModelTypes.B2BioStationA2,
                //    IsCardSupport = true,
                //    IsFaceSupport = false,
                //    IsFingerSupport = true,
                //    IsPasswordSupport = true,
                //    SdkType = SdkType.BioStarV2,
                //    Image = "/Files/Image/BioStationA2.png",
                //    TotalLog = 5000000
                //}, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "BioStation2",
                //    Code = DeviceModelTypes.B2BioStation2,
                //    IsCardSupport = true,
                //    IsFaceSupport = false,
                //    IsFingerSupport = true,
                //    IsPasswordSupport = true,
                //    TotalLog = 3000000,
                //    Image = "/Files/Image/BioStation2.png",
                //    SdkType = SdkType.BioStarV2
                //}, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "BioStation L2",
                //    Code = DeviceModelTypes.B2BioStationL2,
                //    IsCardSupport = true,
                //    IsFingerSupport = true,
                //    IsFaceSupport = false,
                //    IsPasswordSupport = true,
                //    SdkType = SdkType.BioStarV2,
                //    Image = "/Files/Image/BioStationL2.png",
                //    TotalLog = 1000000
                //}, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "BioLite N2",
                //    Code = DeviceModelTypes.B2BioLiteNet2,
                //    IsCardSupport = true,
                //    IsFaceSupport = false,
                //    IsFingerSupport = true,
                //    IsPasswordSupport = true,
                //    SdkType = SdkType.BioStarV2,
                //    Image = "/Files/Image/BioLiteN2.png",
                //    TotalLog = 1000000
                //}, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "BioLite Net",
                //    Code = DeviceModelTypes.B1Biolite,
                //    IsCardSupport = true,
                //    IsFaceSupport = false,
                //    IsFingerSupport = true,
                //    IsPasswordSupport = false,
                //    SdkType = SdkType.BioStarV1,
                //    Image = "/Files/Image/BioLiteNet.png",
                //    TotalLog = 50000
                //}, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "BioEntry W2",
                //    Code = DeviceModelTypes.B2BioEntryW2,
                //    IsCardSupport = true,
                //    IsFaceSupport = false,
                //    IsFingerSupport = true,
                //    IsPasswordSupport = false,
                //    TotalLog = 1000000,
                //    SdkType = SdkType.BioStarV2,
                //    Image = "/Files/Image/BioEntryW2.png"
                //}, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "BioEntry P2",
                //    Code = DeviceModelTypes.B2BioEntryP2,
                //    IsCardSupport = true,
                //    IsFaceSupport = false,
                //    IsFingerSupport = true,
                //    IsPasswordSupport = false,
                //    TotalLog = 1000000,
                //    Image = "/Files/Image/BioEntryP2.png"
                //}, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "Xpass",
                //    Code = DeviceModelTypes.B1Xpass,
                //    IsCardSupport = true,
                //    IsFaceSupport = false,
                //    IsFingerSupport = false,
                //    IsPasswordSupport = false,
                //    SdkType = SdkType.BioStarV1,
                //    Image = "/Files/Image/Xpass.png",
                //    TotalLog = 50000
                //}, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "Xpass S2",
                //    Code = DeviceModelTypes.B1XpassSlim2,
                //    IsCardSupport = true,
                //    IsFaceSupport = false,
                //    IsFingerSupport = true,
                //    IsPasswordSupport = false,
                //    SdkType = SdkType.BioStarV2,
                //    Image = "/Files/Image/XpassSlim2.png",
                //    TotalLog = 100000
                //}, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "BioEntry R2",
                //    Code = DeviceModelTypes.B2BioEntryR2,
                //    IsCardSupport = true,
                //    IsFaceSupport = false,
                //    IsFingerSupport = true,
                //    IsPasswordSupport = false,
                //    SdkType = SdkType.BioStarV2,
                //    Image = "/Files/Image/Bioentryr2.png",
                //    TotalLog = 1000000
                //}, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "Face Lite",
                //    Code = DeviceModelTypes.B2FaceLite,
                //    IsCardSupport = true,
                //    IsFaceSupport = true,
                //    IsFingerSupport = false,
                //    IsPasswordSupport = true,
                //    Image = "/Files/Image/FaceLite.png",
                //    SdkType = SdkType.BioStarV2,
                //    TotalLog = 5000000
                //}, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "FaceStation f2",
                //    Code = DeviceModelTypes.B2F2,
                //    IsCardSupport = true,
                //    IsFaceSupport = true,
                //    IsFingerSupport = true,
                //    IsPasswordSupport = true,
                //    SdkType = SdkType.BioStarV2,
                //    Image = "/Files/Image/F2.png",
                //    TotalLog = 5000000
                //}, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "BioEntry Plus",
                //    Code = DeviceModelTypes.B1BioentryPlus,
                //    IsCardSupport = true,
                //    IsFaceSupport = false,
                //    IsFingerSupport = true,
                //    IsPasswordSupport = false,
                //    SdkType = SdkType.BioStarV1,
                //    Image = "/Files/Image/BioEntryPlus.png",
                //    TotalLog = 50000
                //}, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "DStation",
                //    Code = DeviceModelTypes.B1Dstation,
                //    SdkType = SdkType.BioStarV1,
                //    IsCardSupport = true,
                //    IsFaceSupport = true,
                //    IsFingerSupport = true,
                //    IsPasswordSupport = true,
                //    Image = "/Files/Image/DStation.png",
                //    TotalLog = 1000000
                //}, cancellationToken);

                //await _context.DeviceModels.AddAsync(new DeviceModel
                //{
                //    Name = "BioStation T2",
                //    SdkType = SdkType.BioStarV1,
                //    Code = DeviceModelTypes.B1BioStationT2,
                //    IsCardSupport = true,
                //    IsFaceSupport = false,
                //    IsFingerSupport = true,
                //    IsPasswordSupport = true,
                //    Image = "/Files/Image/BioStationT2.png",
                //    TotalLog = 1000000
                //}, cancellationToken);

                await _context.SaveAsync(cancellationToken);
            }


            if (!await _context.Settings.AnyAsync(cancellationToken))
            {
                await _context.Settings.AddAsync(new Setting
                {
                    FingerPrintQuality = 40,
                    RetryFailedTransferNumber = 3,
                    VitalDevice = 30
                }, cancellationToken);

                await _context.SaveAsync(cancellationToken);
            }
        }
    }
}