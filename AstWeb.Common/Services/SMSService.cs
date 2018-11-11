using AstWeb.Common.Infrastructure;
using AstWeb.Common.Models;
using System;
using AstWeb.Common.Infrastructure;
using AstWeb.Common.Models;
using System;
using System.Text;
using AstWeb.Common.Extension;
using AstWeb.Common.Email;
using AstWeb.Common.Configuration;

namespace AstWeb.Common.Services
{
    public class SMSService : BaseService<SMS, int>
    {
        public Response Add(SMS entity)
        {
            var response = new Response();
            try
            {
                var result = DbBase.Insert(entity);
                if (IsInsertSuccess(result))
                {
                    response.IsSuccess = true;
                    response.Message = "注册成功！";
                    return response;
                }
                response.Message = "注册失败，请检查！";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        public bool IsRight(string phone,string Smscode)
        {
            var md = DbBase.Query<SMS>().Where(o => o.mobile == phone).OrderByDescending(o => o.Id).FirstOrDefault();
            if (md == null) return false;
            if (md.code==Smscode)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
