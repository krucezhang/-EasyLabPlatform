using System.Collections.Generic;
using EasyLab.Server.DTOs;
using System;

namespace EasyLab.Server.Response.Result
{
    public class UsersResult
    {
        public string errorCode { get; set; }

        public string errorType { get; set; }

        public string instrumentId { get; set; }

        public List<CommonUsersResult> instrumentCommonUsers { get; set; }

        public List<AdminUsersResult> instrumentAdminUsers { get; set; }

        public List<User> ToUser(UsersResult userResult)
        {
            List<User> users = new List<User>();

            foreach (var item in userResult.instrumentCommonUsers)
            {
                var model = new User();

                model.UserInfoId = item.userInfoId;
                model.InstrumentId = item.instrumentId;
                model.LoginName = item.loginName;
                model.Password = new EasyLab.Server.Common.Extensions.RijndaelEnhanced().Encrypt(item.password);
                model.Email = IsNull(item.email);
                model.Phone = IsNull(item.phone);
                model.State = IsNull(item.state);
                model.UserType = IsNull(item.userType);
                model.Type = IsNull(item.type);
                model.LoginDateTime = DateTime.Now;
                model.CreateDateTime = DateTime.Now;
                model.Comment = string.Empty;

                users.Add(model);
            }

            foreach (var item in userResult.instrumentAdminUsers)
            {
                var model = new User();

                model.UserInfoId = item.userInfoId;
                model.InstrumentId = item.instrumentId;
                model.LoginName = item.loginName;
                model.Password = new EasyLab.Server.Common.Extensions.RijndaelEnhanced().Encrypt(item.password);
                model.Email = IsNull(item.email);
                model.Phone = IsNull(item.phone);
                model.State = IsNull(item.state);
                model.UserType = IsNull(item.userType);
                model.Type = IsNull(item.type);
                model.LoginDateTime = DateTime.Now;
                model.CreateDateTime = DateTime.Now;
                model.Comment = string.Empty;

                users.Add(model);
            }

            return users;
        }

        private string IsNull(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return value;
        }
    }
}
