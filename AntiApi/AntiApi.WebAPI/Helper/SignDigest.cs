using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Anti.Api.Model
{
    public static class SignDigest
    {
        public static string SignTopRequest(IDictionary<string, string> parameters, string secret)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder();
            query.Append(secret);

            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append("&" + key + "=").Append(value);
                }
            }
            query.Append("&" + secret);
            // 第三步：使用MD5加密
            byte[] result = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            StringBuilder output = new StringBuilder(16);
            for (int i = 0; i < result.Length; i++)
            {
                // convert from hexa-decimal to character   把二进制转化为大写的十六进制
                output.Append((result[i]).ToString("x2", System.Globalization.CultureInfo.InvariantCulture));
            }

            return output.ToString();
        }
    }
}
