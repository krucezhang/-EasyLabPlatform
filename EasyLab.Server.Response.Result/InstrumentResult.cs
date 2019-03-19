using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyLab.Server.DTOs;

namespace EasyLab.Server.Response.Result
{
    public class InstrumentResult
    {
        public string errorCode { get; set; }

        public string errorType { get; set; }

        public List<InstrumentListResult> instrumentList { get; set; }

        public List<DTOs.LabInstrument> ToInstrument(InstrumentResult instrument)
        {
            List<DTOs.LabInstrument> instruments = new List<DTOs.LabInstrument>();

            foreach (var item in instrument.instrumentList)
            {
                var model = new LabInstrument();

                model.RecordId = item.id;
                model.ParentId = "0";
                model.RecordName = item.instrumentName;
                model.cancelAfterTime = item.cancelAfterTime;
                model.cancelPreTime = item.cancelPreTime;
                model.cancelWithoutActionTime = item.cancelWithoutActionTime;
                model.loginPreTime = item.loginPreTime;
                model.userId = item.loginName;

                instruments.Add(model);
            }

            return instruments;
        }
    }

    public class InstrumentListResult
    {
        public string id { get; set; }

        public string instrumentName { get; set; }

        public string parentId { get; set; }

        //Auto cancel reserve when user was lated X minutes
        public int cancelAfterTime { get; set; }

        //it will cancel reserve then it need X hours go aheader time
        public int cancelPreTime { get; set; }

        //In the login status, when user do not anything operate so client would auto logout   
        public int cancelWithoutActionTime { get; set; }

        //Lock login operate before reserve start X minutes for other users do not login
        public int loginPreTime { get; set; }

        public string loginName { get; set; }
    }
}
