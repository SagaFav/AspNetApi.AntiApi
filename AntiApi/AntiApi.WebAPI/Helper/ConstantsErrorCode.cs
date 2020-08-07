using System;
using System.Collections.Generic;
using System.Text;

namespace Anti.Api.Model
{
    public sealed class ConstantsErrorCode
    {

        public const int SUCCESS_CODE_SYS = 200;
        public const string SUCCESS_MSG_SYS = "成功";
        public const int ERR_CODE_PARAM = 400;
        public const string ERR_MSG_PARAM = "请求参数有误";

        public const int ERR_CODE_PARAM_SECURE = 401;
        public const string ERR_MSG_PARAM_SECURE = "安全验证失败";


        public const int ERR_CODE_UNKNOWNERROR = 500;
        public const string ERR_MSG_UNKNOWNERROR = "系统错误";
    }
}
