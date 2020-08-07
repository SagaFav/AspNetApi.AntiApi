# AspNetApi.AntiApi

ASP.Net的WebApi接口验证，

请求：Appkey、AppSeret、Data数据、Timestamp

验签的组装过程：AppSeret & Appkey=Appkey & Data=Data & Timestamp=Timestamp & AppSeret
数据如下：{287c23b482db255b&Appkey=1333&Data=MQ==&Timestamp=20200807114012&287c23b482db255b}
