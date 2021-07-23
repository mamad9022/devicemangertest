using AutoMapper;
using Sepid.DeviceManagerTest.Application.Core.Auth.Command;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.SetNetworkCommand;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Dto;
using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Command.Create;
using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Command.Update;
using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Dto;
using Sepid.DeviceManagerTest.Application.Core.Devices.Command.Create;
using Sepid.DeviceManagerTest.Application.Core.Devices.Command.Update;
using Sepid.DeviceManagerTest.Application.Core.Devices.Dto;
using Sepid.DeviceManagerTest.Application.Core.Groups.Command.CreateGroup;
using Sepid.DeviceManagerTest.Application.Core.Groups.Command.UpdateGroup;
using Sepid.DeviceManagerTest.Application.Core.Groups.Doors.Command.SetDoor;
using Sepid.DeviceManagerTest.Application.Core.Groups.Dto;
using Sepid.DeviceManagerTest.Application.Core.Report.Dto;
using Sepid.DeviceManagerTest.Application.Core.Schedules.Dto;
using Sepid.DeviceManagerTest.Application.Core.Settings.Command.Update;
using Sepid.DeviceManagerTest.Application.Core.Settings.Dto;
using Sepid.DeviceManagerTest.Application.Core.User.Command.EnrollUser;
using Sepid.DeviceManagerTest.Application.Core.User.Command.SendUserToDatabase;
using Sepid.DeviceManagerTest.Application.Core.UserAccessGroups.Command.Upsert;
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Dto.AuthConfig;
using Sepid.DeviceManagerTest.Common.Dto.Door;
using Sepid.DeviceManagerTest.Common.Dto.Schedules;
using System;
using System.Linq;

namespace Sepid.DeviceManagerTest.Application.Common.AutoMapper
{
    public class MappingProfile : Profile
    {


        public MappingProfile()
        {
            #region Device

            CreateMap<Device, DeviceDto>();

            CreateMap<Device, DeviceListDto>()
                .ForMember(x => x.Percentage, opt => opt.MapFrom(des => (double)(des.CurrentLogCount.HasValue ? des.CurrentLogCount : 0) * 100 / des.DeviceModel.TotalLog))
                .ForMember(x => x.TotalLog, opt => opt.MapFrom(des => des.DeviceModel.TotalLog))
                .ForMember(x => x.Image, opt => opt.MapFrom(des => des.DeviceModel.Image))
                .ForMember(x => x.ModelName, opt => opt.MapFrom(des => des.DeviceModel.Name))
                .ForMember(x => x.SdkType, opt => opt.MapFrom(des => des.DeviceModel.SdkType));

            CreateMap<CreateDeviceCommand, Device>().ForMember(x => x.CreateDateTime, opt => opt.MapFrom(des => DateTime.Now)).ForMember(x => x.IsActive, opt => opt.MapFrom(des => true));

            CreateMap<UpdateDeviceCommand, Device>();


            #endregion

            #region DeviceModel

            CreateMap<DeviceModel, DeviceModelDto>();

            CreateMap<CreateDeviceModelCommand, DeviceModel>();

            CreateMap<UpdateDeviceModelCommand, DeviceModel>();


            #endregion

            #region Network

            CreateMap<SetNetWorkCommand, NetworkInfoDto>();

            CreateMap<NetworkInfoDto, Device>()
                .ForMember(x => x.ServerIp, opt => opt.MapFrom(des => des.ServerAddress));

            CreateMap<Device, NetworkInfoDto>()
                .ForMember(x => x.ServerAddress, opt => opt.MapFrom(des => des.ServerIp));


            #endregion

            #region Holiday

            CreateMap<CreateHolidayGroupDto, HolidayGroupDto>();

            CreateMap<CreateHolidayDto, HolidayDto>();


            #endregion

            #region Group

            CreateMap<Group, GroupDto>().ForMember(x => x.Count, opt => opt.MapFrom(des => des.DeviceInGroups.Count)).ForMember(x => x.Children, opt => opt.MapFrom(des => des.Children.OrderBy(x => x.Name)));

            CreateMap<CreateGroupCommand, Group>()
                .ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime.Now));

            CreateMap<UpdateGroupCommand, Group>();

            CreateMap<Group, GroupViewModel>();



            #endregion

            #region Setting

            CreateMap<Setting, SettingDto>();

            CreateMap<UpdateSettingCommand, Setting>();

            #endregion

            CreateMap<TransferLog, TransferLogDto>()
                .ForMember(x => x.DeviceName, opt => opt.MapFrom(des => des.Device.Name))
                .ForMember(x => x.DeviceSerial, opt => opt.MapFrom(des => des.Device.Serial));

            CreateMap<SetAuthConfigCommand, SetAuthConfigDto>();

            CreateMap<SetDoorCommand, CreateDoorDto>();

            CreateMap<DeviceInfo, DeviceSearchDto>();

            CreateMap<UpsertUserAccessGroupCommand, UserAccessGroup>();


            #region User
            CreateMap<EnrollUserCommand, UserDto>();

            CreateMap<UserDto, SendUserToDatabaseCommand>();


            #endregion

            #region redisTemplate


            #endregion
        }
    }
}