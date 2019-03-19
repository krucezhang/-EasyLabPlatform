using EasyLab.Server.DTOs;
using System.Collections.Generic;

namespace EasyLab.Server.Response.Result
{
    public class LabResult
    {
        public string errorCode { get; set; }

        public string errorType { get; set; }

        public List<LabListResult> labList { get; set; }

        public string userInfoId { get; set; }

        public List<DTOs.LabInstrument> ToLab(LabResult lab)
        {
            List<LabInstrument> labInstrument = new List<LabInstrument>();

            foreach (var item in lab.labList)
            {
                var model = new LabInstrument();

                model.RecordId = item.id;
                model.ParentId = "0";
                model.RecordName = item.name;

                labInstrument.Add(model);
            }

            return labInstrument;
        }
    }

    public class LabListResult
    {
        public string id { get; set; }

        public string name { get; set; }

        public string parentId { get; set; }
    }
}
