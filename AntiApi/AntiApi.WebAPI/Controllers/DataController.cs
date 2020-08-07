using Anti.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AntiApi.WebAPI.Controllers
{
    //加上TimingActionFilter，即可添加验证
    [TimingActionFilter]
    [RoutePrefix("api/Data")]
    public class DataController : ApiController
    {

        [HttpPost]
        [Route("TestAPI")]
        public Result TestAPI()
        {
            return Result.SuccessResult("调用成功啦");
        }
    }
}
