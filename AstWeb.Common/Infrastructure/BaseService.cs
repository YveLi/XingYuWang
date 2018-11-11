using System;
using System.Linq;
using System.Text;
using NPoco;
using AstWeb.Common.Infrastructure;

namespace AstWeb.Common.Services
{
    public class BaseService<T, TId> where T : BaseEntity<TId>
    {
        protected Database DbBase { get; }

        public BaseService()
        {
            DbBase = new Database("ConnectionString");
        }

        /// <summary>
        /// 验证数据的正确性，入库的最后一道关卡
        /// </summary>
        /// <param name="entity">实体</param>
        protected void ThrowExceptionIfEntityIsInvalid(T entity)
        {
            if (entity.GetBrokenRules().Any())
            {
                var brokenRules = new StringBuilder();
                brokenRules.AppendLine("数据验证不通过，错误信息：");
                foreach (var businessRule in entity.GetBrokenRules())
                {
                    brokenRules.AppendLine(businessRule.Rule);
                }
                throw new Exception(brokenRules.ToString());
            }
        }


        protected bool IsInsertSuccess(object val)
        {
            return val != null && Convert.ToInt32(val) > 0;
        }

        protected bool IsUpdateSuccess(int val)
        {
            return val > 0;
        }

        public Response Insert(T entity)
        {
            var response = new Response();
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));
                ThrowExceptionIfEntityIsInvalid(entity);
                var result = DbBase.Insert(entity);
                if (result != null && Convert.ToInt32(result) > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "成功";
                    return response;
                }
                response.Message = "失败";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }


}
