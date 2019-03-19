/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            3/06/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using EasyLab.Server.DTOs;
using System;
using System.Collections.Generic;

namespace EasyLab.Server.Business.Interface
{
    public interface IAuditLogAppSvc
    {
        IEnumerable<AuditLog> Get(SearchFilter filter, DateTime? startDate = null, DateTime? endDate = null);

        AuditLog Get(Guid id);

        void Create(AuditLog dto);
    }
}
