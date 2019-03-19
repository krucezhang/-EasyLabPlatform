using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyLab.Server.DTOs;
using EasyLab.Server.Common.Extensions;

namespace EasyLab.Server.Response.Result
{
    public class ReserveQueueResult
    {
        public string errorCode { get; set; }

        public string errorType { get; set; }

        public List<ReserveQueueEntitiesResult> reserveRecordList { get; set; }

        public List<DTOs.ReserveQueue> ToReserverQueue(ReserveQueueResult entity, int maxSort)
        {
            List<ReserveQueue> reserveQueue = new List<ReserveQueue>();

            foreach (var item in entity.reserveRecordList)
            {
                var model = new ReserveQueue();
                maxSort += 1;

                model.reserveUserId = item.userId;
                model.reserveId = item.id;
                model.startDate = Unity.StringToDate(item.startTime);
                model.endDate = Unity.StringToDate(item.endTime);
                model.cancelReserve = Unity.StringToInt(item.cancelTime);
                model.autoCancelReserve = Unity.StringToInt(item.reserveCancelWithoutActionTime);
                model.Sequence = maxSort;
                model.comment = item.comment;

                reserveQueue.Add(model);
            }

            return reserveQueue;
        }
    }

    public class ReserveQueueEntitiesResult
    {
        public string id { get; set; }

        public string startTime { get; set; }

        public string endTime { get; set; }

        public string cancelTime { get; set; }

        public string userId { get; set; }

        public string reserveCancelWithoutActionTime { get; set; }

        public string comment { get; set; }
    }
}
