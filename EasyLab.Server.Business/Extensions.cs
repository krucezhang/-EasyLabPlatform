using EasyLab.Server.Business.Validators;
using EasyLab.Server.DTOs;
using EasyLab.Server.Repository.Interface;
/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/28/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;

namespace EasyLab.Server.Business
{
    internal static class Extensions
    {
        private static SearchFilter DefaultValue = new SearchFilter();

        public static void ProcessWithTransaction(this IUnitOfWork unitOfWork, Action action)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            bool needRollback = false;

            try
            {
                unitOfWork.BeginTransaction();
                needRollback = true;
                action();
                unitOfWork.CommitTransaction();
                needRollback = false;
            }
            catch
            {
                if (needRollback)
                {
                    unitOfWork.RollBackTransaction();
                }

                throw;
            }
        }

        public static SearchFilter ValueOrDefault(this SearchFilter filter)
        {
            if (filter == null)
            {
                return DefaultValue;
            }

            return filter;
        }

        public static DateTime ValueOrNew(this DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return DateTime.UtcNow;
            }

            return date.ToUniversalTime();
        }

        public static Guid ValueOrNew(this Guid id)
        {
            if (id == Guid.Empty)
            {
                return IdentityGenerator.NewSequentialGuid();
            }

            return id;
        }
        /// <summary>
        /// Increase the date time by one accuracy.
        /// </summary>
        /// <param name="time">old time</param>
        /// <returns>old time + Constants.DateTimeAccuracy</returns>
        public static DateTime Increase(this DateTime time)
        {
            return time.AddTicks(Constants.DateTimeAccuracy);
        }
        /// Add an array of new messages of a record to a machine.
        /// </summary>
        /// <param name="messageRepository">message repository</param>
        /// <param name="recordId">record id</param>
        /// <param name="machineId">machine Id</param>
        /// <param name="types">message types</param>
        public static void NewMessage(this IRepository<Data.Models.Message> messageRepository, string recordId, string instrumentId, Guid machineId, params MessageTypes[] types)
        {
            NewMessage(messageRepository, recordId, instrumentId, machineId, null, types);
        }

        public static void NewMessage(this IRepository<Data.Models.Message> messageRepository, string recordId, string instrumentId, Guid machineId, string tag, params MessageTypes[] types)
        {
            DateTime entryDate = DateTime.UtcNow;
            for (int i = 0; i < types.Length; i++)
            {
                entryDate = entryDate.Increase();

                NewMessage(messageRepository, recordId, instrumentId, machineId, entryDate, tag, types[i]);
            }
        }

        public static void NewMessage(this IRepository<Data.Models.Message> messageRepository, string recordId, string instrumentId, Guid machineId, DateTime entryDate, MessageTypes type)
        {
            NewMessage(messageRepository, recordId, instrumentId, machineId, entryDate, null, type);
        }

        public static void NewMessage(this IRepository<Data.Models.Message> messageRepository, string recordId,  string instrumentId, Guid machineId, DateTime entryDate, string tag, MessageTypes type)
        {

            var msg = new Data.Models.Message()
            {
                MessageId = IdentityGenerator.NewSequentialGuid(),
                MachineId = machineId,
                EntryDate = entryDate,
                MessageType = (short)type,
                InstrumentId = instrumentId,
                RecordId = recordId,
                Tag = tag
            };
            messageRepository.Add(msg);
        }
    }
}
