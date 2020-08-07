using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Anti.Api.Model;
using Newtonsoft.Json;

namespace Anti.Api.Model
{
    public class TimingActionFilter : ActionFilterAttribute
    {
        private const string secret = "287c23b482db255b";
        private const string Key = "__action_duration__";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            var json = JsonConvert.SerializeObject(HttpContext.Current.Request.Form.AllKeys.ToDictionary(k => k, k => HttpContext.Current.Request.Form[k]));

            LabelsCmdDto lablesCmd = JsonConvert.DeserializeObject<LabelsCmdDto>(json);
            lablesCmd.Validate();
            if (!Validate(lablesCmd.Sign, lablesCmd.GetParameters()))
            {
                throw new AntiFakeException(ConstantsErrorCode.ERR_CODE_PARAM_SECURE.ToString(), ConstantsErrorCode.ERR_MSG_PARAM_SECURE);
            }

            var stopWatch = new Stopwatch();
            actionContext.Request.Properties[Key] = stopWatch;
            stopWatch.Start();
        }

        internal bool Validate(string sign, IDictionary<string, string> paramDic)
        {
            var temSign = SignDigest.SignTopRequest(paramDic, secret);
            return sign == temSign;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var stopWatch = actionExecutedContext.Request.Properties[Key] as Stopwatch;
            if (stopWatch != null)
            {
                stopWatch.Stop();
            }

        }
    }
}