using EasyLab.Server.DTOs;
using System;
using System.Collections.Generic;

namespace EasyLab.Server.Business.Interface
{
    public interface IReserveQueueSvc
    {
        ReserveQueue Get(string id);

        IEnumerable<Queue> ReserveQueueList();

        ReserveQueue GetPreviousReserveUser();

        ReserveQueue CreateOrUpdate(ReserveQueue dto);

        bool ProcessUpdateQueue();

        string AddOrUpdateReserveQueue(string instrumentId);

        void AutoUpdateReserveQueueState(string instrumentId);

        ReserveQueue AdminCancelReserveRecord(ReserveQueue reserve, string pwd, string username);
    }
}
