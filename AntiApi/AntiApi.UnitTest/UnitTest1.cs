using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Anti.Api.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AntiApi.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //把 AntiApi.WebAPI，部署在iis上，端口号是8070，即可测试调用
            //如果要修改配置Labels_Secret，需要同步修改接口TimingActionFilter.cs的secret
            var result = SendAPI("1", "http://localhost:8070", "/api/Data/TestAPI");
        }


        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="data"></param>
        /// <param name="api"></param>
        private static string SendAPI(string data, string url, string api)
        {
            string Appkey = GetAppSettingsValue("Labels_Appkey");
            string Secret = GetAppSettingsValue("Labels_Secret");
            LabelsCmdDto cmd = new LabelsCmdDto()
            {
                Appkey = Appkey,
                Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(data)),
                Timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"),
            };

            var sign = SignDigest.SignTopRequest(cmd.GetParameters(), Secret);
            cmd.Sign = sign;
            IDictionary<string, string> parameters = new Dictionary<string, string>(cmd.GetParameters());
            parameters.Add("Sign", cmd.Sign);

            WebUtils webUtils = new WebUtils();
            var resultStr = webUtils.DoPost(url + api, parameters);
            return resultStr;
        }

        public static string GetAppSettingsValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return ConfigurationManager.AppSettings[key];
        }
    }

}
