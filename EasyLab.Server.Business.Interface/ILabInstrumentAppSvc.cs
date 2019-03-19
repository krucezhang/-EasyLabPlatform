using EasyLab.Server.DTOs;
using System.Collections.Generic;

namespace EasyLab.Server.Business.Interface
{
    public interface ILabInstrumentAppSvc
    {
        LabInstrument Get(string instrumentId);

        void Create(DTOs.LabInstrument dto);

        List<DTOs.LabInstrument> GetLabInstruments(string id, string userId);

        bool syncInstrumentInfo(string instrumentId);

        bool BindInstrument(string instrumentId);

        bool UnBindInstrument(string instrumentId);
    }
}
