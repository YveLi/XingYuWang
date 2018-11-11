using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace Astrology.Common
{
    public class SMSHelper
    {
        /// <summary>
        /// 模板短信
        /// </summary>
        /// <param name="to">短信接收端手机号码集合，用英文逗号分开，每批发送的手机号数量不得超过100个【如果是测试使用，号码应该填写为测试号码  ps路径：云通讯管理控制台-号码管理-测试号码】</param>
        /// <param name="templateId">模板Id【如果是测试使用，直接填写1，短信内容为：您的验证码为{1}，请于{2}内正确输入，如非本人操作，请忽略此短信。】</param>
        /// <param name="data">可选字段 内容数据，用于替换模板中{序号}【如果是测试使用，对应模板1，内容为{"随机验证码","失效时间"}】</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static Dictionary<string, object> SendSMS(string to, string templateId, string[] data)
        {
            //Dictionary<string, object> Result = new Dictionary<string, object>
            //{
            //    {"statusCode", "000000"},
            //    {"statusMsg", "成功"},
            //    {"data", null}
            //};
            //return Result;
            const string mRestAddress = SysConfig.SMS.RestUrl; //请求地址 默认配置，无需更改
            const string mRestPort = SysConfig.SMS.RestPort; //端口号 默认配置，无需更改
            const string softVer = SysConfig.SMS.SoftVer; //版本号 默认配置，无需更改
            const string mMainAccount = SysConfig.SMS.AccountSid; //开发者主账号 ACCOUNT SID 对应的值
            const string mMainToken = SysConfig.SMS.Authtoken; //开发者主账号 AUTH TOKEN 对应的值
            const string mAppId = SysConfig.SMS.AppId; //应用管理-应用列表 应用 APP ID 对应的值
            string type = SysConfig.SMS.Type; //type参数，可供你选择xml格式请求，或者是json请求（没有特殊要去，可忽略此参数，认定一种格式就行）

            try
            {
                string date = DateTime.Now.ToString("yyyyMMddhhmmss");
                string sigstr = CommFun.Md5(mMainAccount + mMainToken + date);
                string uriStr = string.Format("https://{0}:{1}/{2}/Accounts/{3}/SMS/TemplateSMS?sig={4}", mRestAddress,
                    mRestPort, softVer, mMainAccount, sigstr);
                Uri address = new Uri(uriStr);

                // 创建网络请求  
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

                // 构建Head
                if (request != null)
                {
                    request.Method = "POST";

                    Encoding myEncoding = Encoding.GetEncoding("utf-8");
                    byte[] myByte = myEncoding.GetBytes(mMainAccount + ":" + date);
                    string authStr = Convert.ToBase64String(myByte);
                    request.Headers.Add("Authorization", authStr);


                    // 构建Body
                    StringBuilder bodyData = new StringBuilder();

                    if (type == "xml")
                    {
                        request.Accept = "application/xml";
                        request.ContentType = "application/xml;charset=utf-8";
                        bodyData.Append("<?xml version='1.0' encoding='utf-8'?><TemplateSMS>");
                        bodyData.Append("<to>").Append(to).Append("</to>");
                        bodyData.Append("<appId>").Append(mAppId).Append("</appId>");
                        bodyData.Append("<templateId>").Append(templateId).Append("</templateId>");
                        if (data != null && data.Length > 0)
                        {
                            bodyData.Append("<datas>");
                            foreach (string item in data)
                            {
                                bodyData.Append("<data>").Append(item).Append("</data>");
                            }
                            bodyData.Append("</datas>");
                        }
                        bodyData.Append("</TemplateSMS>");
                    }
                    else
                    {
                        request.Accept = "application/json";
                        request.ContentType = "application/json;charset=utf-8";
                        bodyData.Append("{");
                        bodyData.Append("\"to\":\"").Append(to).Append("\"");
                        bodyData.Append(",\"appId\":\"").Append(mAppId).Append("\"");
                        bodyData.Append(",\"templateId\":\"").Append(templateId).Append("\"");
                        if (data != null && data.Length > 0)
                        {
                            bodyData.Append(",\"datas\":[");
                            int index = 0;
                            foreach (string item in data)
                            {
                                if (index == 0)
                                {
                                    bodyData.Append("\"" + item + "\"");
                                }
                                else
                                {
                                    bodyData.Append(",\"" + item + "\"");
                                }
                                index++;
                            }
                            bodyData.Append("]");
                        }
                        bodyData.Append("}");

                    }

                    byte[] byteData = UTF8Encoding.UTF8.GetBytes(bodyData.ToString());

                    // 开始请求
                    using (Stream postStream = request.GetRequestStream())
                    {
                        postStream.Write(byteData, 0, byteData.Length);
                    }

                    // 获取请求
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        // Get the response stream  
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string responseStr = reader.ReadToEnd();
                        if (responseStr.Length > 0)
                        {
                            Dictionary<string, object> responseResult = new Dictionary<string, object>
                            {
                                {"statusCode", "0"},
                                {"statusMsg", "成功"},
                                {"data", null}
                            };
                            if (type == "xml")
                            {
                                XmlDocument resultXml = new XmlDocument();
                                resultXml.LoadXml(responseStr);
                                XmlNodeList nodeList = resultXml.SelectSingleNode("Response").ChildNodes;
                                foreach (XmlNode item in nodeList)
                                {
                                    switch (item.Name)
                                    {
                                        case "statusCode":
                                            responseResult["statusCode"] = item.InnerText;
                                            break;
                                        case "statusMsg":
                                            responseResult["statusMsg"] = item.InnerText;
                                            break;
                                        case "TemplateSMS":
                                            {
                                                Dictionary<string, object> retData = item.ChildNodes.Cast<XmlNode>().ToDictionary<XmlNode, string, object>(subItem => subItem.Name, subItem => subItem.InnerText);
                                                responseResult["data"] = new Dictionary<string, object> { { item.Name, retData } };
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                responseResult.Clear();
                                responseResult["resposeBody"] = responseStr;
                            }

                            return responseResult;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new Dictionary<string, object>
                        {
                            {"statusCode", 172002},
                            {"statusMsg", "无返回"},
                            {"data", null}
                        };
        }

        //APi调用实例

        //readonly ApiRoot _api = new ApiRoot();
        //readonly string _mobilePhone = string.Empty;
        //readonly string _verifCode = string.Empty;
        //readonly SmsHelper _sms = new SmsHelper();
        //readonly SMSBll _smsBll = new SMSBll();

        ///// <summary>
        ///// 发送短信验证码
        ///// </summary>
        //[AcceptVerbs("GET", "POST")]
        //public void SendVerifCode(string aid, string token, string appId, string phoneNum)
        //{
        //    if (aid == SysConfig.Sms.AccountSid
        //        && token == SysConfig.Sms.Authtoken
        //        && appId == SysConfig.Sms.AppId
        //        && (phoneNum.Length == 11))
        //    {
        //        if (UserHelper.IsExitUsers(phoneNum))
        //        {
        //            _api.Status = (int)ApiConfig.Status.Defeat;
        //            _api.Msg = "用户已存在系统中";
        //        }
        //        else
        //        {
        //            string randNum = CommFun.GetRndNum(5);
        //            Dictionary<string, object> dic = _sms.SendTemplateSms(phoneNum, SysConfig.Sms.TempleId, new[] { randNum, "5" });
        //            if (dic["resposeBody"].ToString().Contains("000000"))//有时间再优化
        //            {
        //                _api.Status = (int)ApiConfig.Status.Success;
        //                _api.Msg = SysConfig.IsPl
        //                    ? "验证码已发送到您的手机 "
        //                    : string.Format("当前验证码为：{0}", randNum);
        //                //存储验证码
        //                _smsBll.Add(new SMS
        //                {
        //                    PhoneNum = phoneNum,
        //                    Code = randNum
        //                });
        //            }
        //            else
        //            {
        //                _api.Status = (int)ApiConfig.Status.Defeat;
        //                _api.Msg = dic["resposeBody"].ToString();
        //            }
        //        }

        //    }
        //    else
        //    {
        //        _api.Status = (int)ApiConfig.Status.Defeat;
        //        _api.Msg = "一次八分钱了，别乱来！";
        //        _api.Result = "";
        //    }
        //    WebUtils.ResponseHTML(AjaxHelper.JsonpFormat(_api));
        //}
        ///// <summary>
        ///// 验证
        ///// </summary>
        ///// <param name="uid"></param>
        ///// <param name="mobile"></param>
        ///// <param name="code"></param>
        //[AcceptVerbs("GET", "POST")]
        //public void VerifMobile(string uid, string mobile, string code)
        //{
        //    if (_mobilePhone != mobile)
        //    {
        //        _api.Status = (int)ApiConfig.Status.Defeat;
        //        _api.Msg = "手机错误";
        //    }
        //    else
        //    {
        //        if (code != _verifCode)
        //        {
        //            _api.Status = (int)ApiConfig.Status.Defeat;
        //            _api.Msg = "验证码错误";
        //        }
        //        else
        //        {
        //            _api.Status = (int)ApiConfig.Status.Success;
        //            _api.Msg = "验证成功";
        //        }
        //    }
        //    WebUtils.ResponseHTML(AjaxHelper.JsonpFormat(_api));
        //}
    }
}
