using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Anti.Api.Model
{
    public class AuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request;

            #region 请求头Header
            Result result = new Result();
            DateTime timestamp = DateTime.MinValue;
            var appKey = string.Empty;
            var token = string.Empty;
            var sign = string.Empty;
            var appKeyConfig = ConfigurationManager.AppSettings["appkey"].ToString();
            var tokenConfig = ConfigurationManager.AppSettings["token"].ToString();
            var appSecretConfig = ConfigurationManager.AppSettings["appsecret"].ToString();
            if (request.Headers.Contains("timestamp"))
            {
                if (!DateTime.TryParse(HttpUtility.UrlDecode(request.Headers.GetValues("timestamp").FirstOrDefault()), out timestamp))
                {
                    result = Result.ErrorResult("签名失败，时间戳格式不合法");
                    actionContext.Response = new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(result))
                    };
                    return;
                }
                else
                {
                    DateTime nowDateTime = DateTime.Now;
                    if ((nowDateTime - timestamp).Minutes > 5)
                    {
                        result = Result.ErrorResult("签名失败，接口已过期！");
                        actionContext.Response = new HttpResponseMessage()
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(result))
                        };
                        return;
                    }
                }
            }
            if (request.Headers.Contains("appkey"))
            {
                appKey = HttpUtility.UrlDecode(request.Headers.GetValues("appkey").FirstOrDefault());
                if (appKey != appKeyConfig)
                {
                    result = Result.ErrorResult("签名失败，appkey不合法");
                    actionContext.Response = new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(result))
                    };
                    return;
                }
            }
            if (request.Headers.Contains("token"))
            {
                token = HttpUtility.UrlDecode(request.Headers.GetValues("token").FirstOrDefault());
                if (token != tokenConfig)
                {
                    result = Result.ErrorResult("签名失败，token不合法");
                    actionContext.Response = new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(result))
                    };
                    return;
                }
            }
            if (request.Headers.Contains("sign"))
            {
                sign = HttpUtility.UrlDecode(request.Headers.GetValues("sign").FirstOrDefault());
            }


            #endregion

            if (timestamp == DateTime.MinValue || string.IsNullOrEmpty(appKey) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(sign))
            {
                result = Result.ErrorResult("签名失败，系统参数不合法！");
                actionContext.Response = new HttpResponseMessage()
                {
                    Content = new StringContent(JsonConvert.SerializeObject(result))
                };
                return;
            }
            else//签名认证
            {
                var businessData = string.Empty;
                if (request.Method.ToString().ToLower() == "get")
                {
                    NameValueCollection nameValueCollection = HttpContext.Current.Request.QueryString;
                    SortedDictionary<string, string> sortedDic = new SortedDictionary<string, string>();
                    for (int i = 0; i < nameValueCollection.Count; i++)
                    {
                        var key = nameValueCollection.Keys[i];
                        sortedDic.Add(key, nameValueCollection[key]);
                    }
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var key in sortedDic.Keys)
                    {
                        stringBuilder.Append(key).Append(sortedDic[key]);
                    }
                    businessData = stringBuilder.ToString();
                }
                else if (request.Method.ToString().ToLower() == "post")
                {
                    var stream = HttpContext.Current.Request.InputStream;
                    stream.Position = 0;
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        businessData = reader.ReadToEnd();
                    }
                }
                SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("timestamp", timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                dic.Add("appkey", appKey);
                dic.Add("token", token);
                dic.Add("businessdata", businessData);//业务报文
                dic.Add("appsecret", appSecretConfig);//商家密钥
                var signature = GenerateSignature(dic);
                if (sign != signature)
                {
                    result = Result.ErrorResult("签名错误！");
                    actionContext.Response = new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(result))
                    };
                    return;
                }
            }
            base.OnActionExecuting(actionContext);
        }

        #region 计算签名
        /// <summary>
        /// 计算签名
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        private string GenerateSignature(SortedDictionary<String, String> dic)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in dic.Keys)
            {
                if (string.IsNullOrEmpty(item) || string.IsNullOrEmpty(dic[item]))
                {
                    continue;
                }
                builder.Append(item).Append(dic[item]);
            }
            return MD5(builder.ToString());
        }

        public static string MD5(string pwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(pwd);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            string str = "";
            for (int i = 0; i < md5data.Length; i++)
            {
                str += md5data[i].ToString("x").PadLeft(2, '0');
            }
            return str;
        }
        #endregion
    }
}