using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Anti.Api.Model
{
    public class HttpHeaderFilter:IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, System.Web.Http.Description.ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();
            var filterPipeline = apiDescription.ActionDescriptor.GetFilterPipeline();
            //判断是否添加方法过滤器
            var isActionFilter = filterPipeline.Select(filterInfo => filterInfo.Instance).Any(filter => filter is AuthAttribute);
            //判断是否允许匿名方法 
            var allowAnonymous = apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
            if (isActionFilter && !allowAnonymous)
            {
                operation.parameters.Add(new Parameter { name = "appkey", @in = "header", description = "appkey", required = false, type = "string", @default="test"});
                operation.parameters.Add(new Parameter { name = "token", @in = "header", description = "token", required = false, type = "string", @default = "test" });
                operation.parameters.Add(new Parameter { name = "timestamp", @in = "header", description = "时间戳", required = false, type = "string" , @default = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                operation.parameters.Add(new Parameter { name = "sign", @in = "header", description = "签名", required = false, type = "string" , @default = "test" });
            }
        }
    }
}