using Anti.Api.Model;
using AntiApi.WebAPI.Model;
using Newtonsoft.Json;
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
        public LabelsQueryDto TestAPI([FromBody] LabelsCmdDto cmd)
        {
            var request = JsonConvert.DeserializeObject<StoreRequest>(Base64Helper.Base64Decode(cmd.Data));

            #region 参数判断

            if (request == null)
            {
                return new LabelsQueryDto() { Code = ConstantsErrorCode.ERR_CODE_PARAM, Msg = "请求参数不能为空" };
            }
            if (string.IsNullOrEmpty(request.UserNo))
            {
                return new LabelsQueryDto() { Code = ConstantsErrorCode.ERR_CODE_PARAM, Msg = "客户编号不能为空" };
            }
            if (string.IsNullOrEmpty(request.StoreName))
            {
                return new LabelsQueryDto() { Code = ConstantsErrorCode.ERR_CODE_PARAM, Msg = "店铺名称不能为空" };
            }

            #endregion

            #region 业务代码

            //TODO:待定

            #endregion

            return new LabelsQueryDto() { Code = ConstantsErrorCode.SUCCESS_CODE_SYS, Msg = "调用成功啦，这是请求参数：" + JsonConvert.SerializeObject(request) };

        }
    }
}
