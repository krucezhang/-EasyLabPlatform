using AutoMapper;
using AutoMapper.Mappers;
/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/15/2015
* Revision:        0.1       Draft
*                  
************************************************************/

namespace EasyLab.Server.Conversions
{
    /// <summary>
    /// Extensions methods for DTOs classes
    /// </summary>
    public static class DTOExtensions
    {
        public static EasyLab.Server.Data.Models.User ToData(this EasyLab.Server.DTOs.User dto)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.DTOs.User, EasyLab.Server.Data.Models.User>()
                .ForMember(o => o.UserId, m => m.MapFrom(s => s.UserId))
                .ForMember(o => o.UserInfoId, m => m.MapFrom(s => s.UserInfoId))
                .ForMember(o => o.LoginName, m => m.MapFrom(s => s.LoginName))
                .ForMember(o => o.Password, m => m.MapFrom(s => s.Password))
                .ForMember(o => o.Email, m => m.MapFrom(s => s.Email))
                .ForMember(o => o.Phone, m => m.MapFrom(s => s.Phone))
                .ForMember(o => o.InstrumentId, m => m.MapFrom(s => s.InstrumentId))
                .ForMember(o => o.State, m => m.MapFrom(s => s.State))
                .ForMember(o => o.Type, m => m.MapFrom(s => s.Type))
                .ForMember(o => o.UserType, m => m.MapFrom(s => s.UserType))
                .ForMember(o => o.CreateDateTime, m => m.MapFrom(s => s.CreateDateTime))
                .ForMember(o => o.LoginDateTime, m => m.MapFrom(s => s.LoginDateTime))
                .ForMember(o => o.Comment, m => m.MapFrom(s => s.Comment));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.Data.Models.User>(dto);
            }
        }

        public static EasyLab.Server.Data.Models.LabAttribute ToDto(this EasyLab.Server.DTOs.LabAttribute dto)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.DTOs.LabAttribute, EasyLab.Server.Data.Models.LabAttribute>()
                .ForMember(o => o.LabId, m => m.MapFrom(s => s.LabId))
                .ForMember(o => o.userId, m => m.MapFrom(s => s.userId))
                .ForMember(o => o.MaterialName, m => m.MapFrom(s => s.MaterialName))
                .ForMember(o => o.MaterialAttribute, m => m.MapFrom(s => s.MaterialAttribute));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.Data.Models.LabAttribute>(dto);
            }
        }

        public static EasyLab.Server.Data.Models.GlobalSetting ToData(this EasyLab.Server.DTOs.GlobalSetting dto)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.DTOs.GlobalSetting, EasyLab.Server.Data.Models.GlobalSetting>()
                .ForMember(o => o.Category, m => m.MapFrom(s => s.Category))
                .ForMember(o => o.OptionKey, m => m.MapFrom(s => s.OptionKey))
                .ForMember(o => o.OptionValue, m => m.MapFrom(s => s.OptionValue));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.Data.Models.GlobalSetting>(dto);
            }
        }

        public static EasyLab.Server.Data.Models.DeviceSetting ToData(this EasyLab.Server.DTOs.DeviceSetting dto)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.DTOs.DeviceSetting, EasyLab.Server.Data.Models.DeviceSetting>();

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.Data.Models.DeviceSetting>(dto);
            }
        }

        public static EasyLab.Server.Data.Models.Message ToData(this EasyLab.Server.DTOs.Message dto)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.DTOs.Message, EasyLab.Server.Data.Models.Message>()
                .ForMember(o => o.MessageId, m => m.MapFrom(s => s.Id));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.Data.Models.Message>(dto);
            }
        }

        public static EasyLab.Server.Data.Models.LabInstrument ToData(this EasyLab.Server.DTOs.LabInstrument dto)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.DTOs.LabInstrument, EasyLab.Server.Data.Models.LabInstrument>()
                .ForMember(o => o.LabInstrumentId, m => m.MapFrom(s => s.Id));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.Data.Models.LabInstrument>(dto);
            }
        }

        public static EasyLab.Server.Data.Models.ReserveQueue ToData(this EasyLab.Server.DTOs.ReserveQueue dto)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.DTOs.ReserveQueue, EasyLab.Server.Data.Models.ReserveQueue>()
                .ForMember(o => o.queueId, m => m.MapFrom(s => s.Id));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.Data.Models.ReserveQueue>(dto);
            }
        }

        public static EasyLab.Server.Data.Models.AuditLog ToData(this EasyLab.Server.DTOs.AuditLog dto)
        {
            if (dto == null || dto.Log == null)
            {
                return null;
            }

            var model = dto.Log.ToAuditLogData();
            if (dto.User != null)
            {
                model.UserId = dto.User.UserInfoId;
            }
            if (dto.Application != null)
            {
                model.ApplicationId = dto.Application.Id ?? string.Empty;
            }
            else
            {
                model.ApplicationId = string.Empty;
            }

            return model;
        }

        public static EasyLab.Server.Data.Models.AuditLog ToAuditLogData(this EasyLab.Server.DTOs.Log dto)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.DTOs.Log, EasyLab.Server.Data.Models.AuditLog>()
                .ForMember(o => o.AuditLogId, m => m.MapFrom(s => s.Id))
                .ForMember(o => o.ResourceAction, m => m.MapFrom(s => s.ResourceAction ?? string.Empty))
                .ForMember(o => o.ResourceType, m => m.MapFrom(s => s.ResourceType ?? string.Empty))
                .ForMember(o => o.ResourceValue, m => m.MapFrom(s => s.ResourceValue ?? string.Empty))
                .ForMember(o => o.ResourceType2, m => m.MapFrom(s => s.ResourceType2 ?? string.Empty))
                .ForMember(o => o.ResourceValue2, m => m.MapFrom(s => s.ResourceValue2 ?? string.Empty));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.Data.Models.AuditLog>(dto);
            }
        }

        private static ConfigurationStore CreateConfigurationStore()
        {
            return new ConfigurationStore(new TypeMapFactory(), MapperRegistry.AllMappers());
        }
    }
}
