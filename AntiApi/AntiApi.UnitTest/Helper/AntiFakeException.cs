using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Anti.Api.Model
{
    /// <summary>
    /// 客户端异常。
    /// </summary>
    public class AntiFakeException : Exception
    {
        private string errorCode;
        private string errorMsg;

        public AntiFakeException()
            : base()
        {
        }

        public AntiFakeException(string message)
            : base(message)
        {
        }

        protected AntiFakeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public AntiFakeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AntiFakeException(string errorCode, string errorMsg)
            : base(errorCode + ":" + errorMsg)
        {
            this.errorCode = errorCode;
            this.errorMsg = errorMsg;
        }

        public string ErrorCode
        {
            get { return this.errorCode; }
        }

        public string ErrorMsg
        {
            get { return this.errorMsg; }
        }
    }
}
