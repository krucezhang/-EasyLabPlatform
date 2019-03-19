/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            2/5/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using EasyLab.Server.DTOs;

namespace EasyLab.Server.Business.Interface
{
    public interface IGlobalSettingAppSvc
    {
        GlobalSetting Get(string category, string optionKey);

        bool CheckOnlineClient(string instrumentId);

    }
}
