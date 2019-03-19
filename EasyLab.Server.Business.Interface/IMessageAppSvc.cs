using EasyLab.Server.DTOs;
using System;

namespace EasyLab.Server.Business.Interface
{
    public interface IMessageAppSvc
    {
        Message Get(Guid id);

        void Create(Message dto);
    }
}
