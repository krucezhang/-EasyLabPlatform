using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyLab.Server.DTOs;

namespace EasyLab.Server.Business.Interface
{
    public interface IUserAppSvc
    {
        User Get(Guid Id);

        IEnumerable<User> Get();

        void Create(User dto);

        string VerityManagerAccount(string username, string password);

        string checkUserLoginAuthority(string userInfoId);

        string GetUserInitListByInstrument(string instrumentId);
    }
}
