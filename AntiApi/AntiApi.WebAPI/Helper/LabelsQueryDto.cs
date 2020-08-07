using System;
using System.Collections.Generic;
using System.Text;

namespace Anti.Api.Model
{
    public class LabelsQueryDto
    {
        /// <summary>
        /// 返回状态码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回相关信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public string Data { get; set; }


        public static LabelsQueryDto SuccessResult()
        {
            return new LabelsQueryDto
            {
                Code = ConstantsErrorCode.SUCCESS_CODE_SYS,
                Msg = ConstantsErrorCode.SUCCESS_MSG_SYS
            };
        }
    }
}
