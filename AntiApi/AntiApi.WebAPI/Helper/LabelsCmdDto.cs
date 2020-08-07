using System;
using System.Collections.Generic;
using System.Text;

namespace Anti.Api.Model
{
    public class LabelsCmdDto
    {
        public string Appkey { get; set; }

        public string Data { get; set; }
        /// <summary>
        /// yyyyMMddHHmmss
        /// </summary>
        public string Timestamp { get; set; }

        public string Sign { get; set; }


        public IDictionary<string, string> GetParameters()
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Appkey", this.Appkey.ToString());
            parameters.Add("Data", this.Data.ToString());
            parameters.Add("Timestamp", this.Timestamp.ToString());
            return parameters;
        }


        public void Validate()
        {
            RequestValidator.ValidateRequired("Appkey", this.Appkey);
            RequestValidator.ValidateRequired("Data", this.Data);
            RequestValidator.ValidateRequired("Timestamp", this.Timestamp);
            RequestValidator.ValidateRequired("Sign", this.Sign);
        }
    }
}
