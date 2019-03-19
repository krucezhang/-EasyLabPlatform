using AutoMapper;
using AutoMapper.Mappers;
/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/15/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;

namespace EasyLab.Server.Conversions
{
    /// <summary>
    /// Extensions methods for data model classes
    /// </summary>
    public static class DataModelExtensions
    {
        private static readonly EasyLab.Server.DTOs.User EasyLabAccount = new DTOs.User()
        {
            UserId = new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"),
            LoginName = "EasyLabUser",
            Password = string.Empty,
            Comment = string.Empty
        };

        public static EasyLab.Server.DTOs.User ToDto(this EasyLab.Server.Data.Models.User data)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.Data.Models.User, EasyLab.Server.DTOs.User>()
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
                return engine.Map<EasyLab.Server.DTOs.User>(data);
            }
        }

        public static EasyLab.Server.DTOs.LabAttribute ToDto(this EasyLab.Server.Data.Models.LabAttribute data)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.Data.Models.LabAttribute, EasyLab.Server.DTOs.LabAttribute>()
                .ForMember(o => o.LabId, m => m.MapFrom(s => s.LabId))
                .ForMember(o => o.userId, m => m.MapFrom(s => s.userId))
                .ForMember(o => o.MaterialName, m => m.MapFrom(s => s.MaterialName))
                .ForMember(o => o.MaterialAttribute, m => m.MapFrom(s => s.MaterialAttribute));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.DTOs.LabAttribute>(data);
            }
        }

        public static EasyLab.Server.DTOs.GlobalSetting ToDto(this EasyLab.Server.Data.Models.GlobalSetting data)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.Data.Models.GlobalSetting, EasyLab.Server.DTOs.GlobalSetting>()
                .ForMember(o => o.Category, m => m.MapFrom(s => s.Category))
                .ForMember(o => o.OptionKey, m => m.MapFrom(s => s.OptionKey))
                .ForMember(o => o.OptionValue, m => m.MapFrom(s => s.OptionValue));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.DTOs.GlobalSetting>(data);
            }
        }

        public static EasyLab.Server.DTOs.Message ToDto(this EasyLab.Server.Data.Models.Message data)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.Data.Models.Message, EasyLab.Server.DTOs.Message>()
                .ForMember(o => o.Id, m => m.MapFrom(s => s.MessageId))
                .ForMember(o => o.InstrumentId, m => m.MapFrom(s => s.Machine == null ? null : s.Machine.InstrumentId))
                .ForMember(o => o.InstrumentName, m => m.MapFrom(s => s.Machine == null ? null : s.Machine.ComputerName))
                .ForMember(o => o.InstrumentIpV4, m => m.MapFrom(s => s.Machine == null ? null : s.Machine.IpV4Address));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.DTOs.Message>(data);
            }
        }

        public static EasyLab.Server.DTOs.AuditLog ToDto(this EasyLab.Server.Data.Models.AuditLog data)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.Data.Models.AuditLog, EasyLab.Server.DTOs.AuditLog>()
                .ForMember(o => o.Application, m => m.MapFrom(s => s.Application.ToDto()))
                .ForMember(o => o.Log, m => m.MapFrom(s => s.ToLogDto()))
                .ForMember(o => o.User, m => m.MapFrom(s => GetUser(s.User, s.UserId)));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.DTOs.AuditLog>(data);
            }
        }

        public static EasyLab.Server.DTOs.Application ToDto(this EasyLab.Server.Data.Models.Application data)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.Data.Models.Application, EasyLab.Server.DTOs.Application>()
                .ForMember(o => o.Id, m => m.MapFrom(s => s.ApplicationId));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.DTOs.Application>(data);
            }
        }

        public static EasyLab.Server.DTOs.LabInstrument ToDto(this EasyLab.Server.Data.Models.LabInstrument data)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.Data.Models.LabInstrument, EasyLab.Server.DTOs.LabInstrument>()
                .ForMember(o => o.Id, m => m.MapFrom(s => s.LabInstrumentId));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.DTOs.LabInstrument>(data);
            }
        }

        public static EasyLab.Server.DTOs.Log ToLogDto(this EasyLab.Server.Data.Models.AuditLog data)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.Data.Models.AuditLog, EasyLab.Server.DTOs.Log>()
                .ForMember(o => o.Id, m => m.MapFrom(s => s.AuditLogId))
                .ForMember(o => o.CreateDate, m => m.MapFrom(s => new DateTime(s.CreateDate.Ticks, DateTimeKind.Utc)));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.DTOs.Log>(data);
            }
        }

        public static EasyLab.Server.DTOs.DeviceSetting ToDto(this EasyLab.Server.Data.Models.DeviceSetting data)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.Data.Models.DeviceSetting, EasyLab.Server.DTOs.DeviceSetting>();

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.DTOs.DeviceSetting>(data);
            }
        }

        public static EasyLab.Server.DTOs.ReserveQueue ToDto(this EasyLab.Server.Data.Models.ReserveQueue data)
        {
            var config = CreateConfigurationStore();

            config.CreateMap<EasyLab.Server.Data.Models.ReserveQueue, EasyLab.Server.DTOs.ReserveQueue>()
                .ForMember(o => o.Id, m => m.MapFrom(s => s.queueId));

            using (var engine = new MappingEngine(config))
            {
                return engine.Map<EasyLab.Server.DTOs.ReserveQueue>(data);
            }
        }

        private static EasyLab.Server.DTOs.User GetUser(EasyLab.Server.Data.Models.User account, string accountId)
        {
            if (account != null)
            {
                return account.ToDto();
            }

            if (accountId == EasyLabAccount.UserInfoId)
            {
                return EasyLabAccount;
            }

            return null;
        }

        private static ConfigurationStore CreateConfigurationStore()
        {
            return new ConfigurationStore(new TypeMapFactory(), MapperRegistry.AllMappers());
        }
    }
}
