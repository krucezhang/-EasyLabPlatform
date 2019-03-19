/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            3/06/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyLab.Server.DTOs;
using EasyLab.Server.Business.Interface;
using EasyLab.Server.Repository.Interface;
using EasyLab.Server.Common.Errors;
using EasyLab.Server.Conversions;
using EasyLab.Server.Business.Validators;

namespace EasyLab.Server.Business
{
    public class AuditLogAppSvc : IAuditLogAppSvc
    {
        private IUnitOfWork unitOfWork;
        private IRepository<Data.Models.AuditLog> auditLogRepository;
        private IRepository<Data.Models.Message> messageRepository;
        private IRepository<Data.Models.DeviceSetting> deviceSettingRepository;

        public AuditLogAppSvc(IUnitOfWork unitOfWork,
            IRepository<Data.Models.Message> messageRepository,
            IRepository<Data.Models.AuditLog> auditLogRepository,
            IRepository<Data.Models.DeviceSetting> deviceSettingRepository)
        {
            this.unitOfWork = unitOfWork;

            this.auditLogRepository = auditLogRepository;
            this.auditLogRepository.UnitOfWork = unitOfWork;

            this.messageRepository = messageRepository;
            this.messageRepository.UnitOfWork = unitOfWork;

            this.deviceSettingRepository = deviceSettingRepository;
            this.deviceSettingRepository.UnitOfWork = unitOfWork;
        }

        public IEnumerable<AuditLog> Get(SearchFilter filter, DateTime? startDate, DateTime? endDate)
        {
            filter = filter.ValueOrDefault();

            //string sql;
            //List<object> parameters;

            //var items = this.unitOfWork.SqlQuery<Data.Models.AuditLogSearchResult>(sql, parameters.ToArray()).ToList();
            var items = new List<string>();

            var result = new List<AuditLog>(items.Count);
            foreach (var item in items)
            {
                //result.Add(item.ToLogUTC());
            }

            return result;
        }

        public AuditLog Get(Guid id)
        {
            var data = auditLogRepository.GetByKey(id);

            return data.ToDto();
        }

        public void Create(AuditLog dto)
        {
            ThrowHelper.ThrowArgumentNullExceptionIfNull(dto, "dto");
            ThrowHelper.ThrowArgumentNullExceptionIfNull(dto.Log, "dto.log");

            //var validator = new AuditLogValidator();
            //validator.ValidateAndThrowEasyLabException(dto);

            var data = dto.ToData();
            data.CreateDate = data.CreateDate.ValueOrNew();
            data.AuditLogId = data.AuditLogId.ValueOrNew();
            data.InstrumentId = GetStationIdFromDeviceSettings();

            unitOfWork.ProcessWithTransaction(() =>
                auditLogRepository.Add(data)
            );

            dto.Log.Id = data.AuditLogId;
        }

        private string GetStationIdFromDeviceSettings()
        {
            var stationIdSetting = this.deviceSettingRepository.GetByKey("common", "InstrumentId");

            if (stationIdSetting == null || string.IsNullOrWhiteSpace(stationIdSetting.OptionValue))
            {
                return null;
            }

            return stationIdSetting.OptionValue;
        }
    }
}
