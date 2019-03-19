using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AttributeRouting;
using AttributeRouting.Web.Http;
using System.Web.Http;
using System.Net.Http;
using EasyLab.Server.REST.Services.Models;
using EasyLab.Server.Business.Interface;
using EasyLab.Server.DTOs;
using EasyLab.Server.Common.Errors;
using EasyLab.Server.Resources;


namespace EasyLab.Server.REST.Services.Controllers
{
    [DefaultHttpRouteConvention]
    [RoutePrefix(Constants.UriRoot + "users")]
    public class UsersController : ApiController
    {
        private IUserAppSvc appSvc;

        public UsersController(IUserAppSvc appSvc)
        {
            this.appSvc = appSvc;
        }

        public IEnumerable<User> Get()
        {
            var data = appSvc.Get();
            return data;
        }

        [GET("{id}", RouteName = "users_get_by_id")]
        public User Get(Guid id)
        {
            var data = appSvc.Get(id);
            if (data == null)
            {
                throw new EasyLabException(System.Net.HttpStatusCode.NotFound, Rs.INVALID_RESOURCE_ID);
            }

            return data;
        }

        public HttpResponseMessage Post(User model)
        {
            if (model == null)
            {
                throw new EasyLabException(System.Net.HttpStatusCode.BadRequest, Rs.MUST_NOT_BE_BLANK);
            }

            appSvc.Create(model);

            var response = Request.CreateResponse<User>(System.Net.HttpStatusCode.Created, model);
            string url = Url.Link("users_get_by_id", new { @id = model.UserId });
            response.Headers.Location = new Uri(url);

            return response;
        }
    }
}
