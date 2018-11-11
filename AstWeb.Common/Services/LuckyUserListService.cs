using AstWeb.Common.Infrastructure;
using AstWeb.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Services
{
    public class LuckyUserListService : BaseService<LuckyUserList, int>
    {
        public IList<LuckyUserList> GetLuckyUserList(int wid, int activityid, string opid)
        {
            return DbBase.Query<LuckyUserList>().Where(o => o.Wid == wid && o.ActivityId == activityid && o.WechatId == opid).ToList();
        }
        public int getcount()
        {
            return DbBase.Query<LuckyUserList>().Count();
        }
        public GetListsResponse<LuckyUserList> GetLuckTop()
        {
            var response = new GetListsResponse<LuckyUserList>();
            var result = DbBase.Query<LuckyUserList>().OrderByDescending(o => o.Id).ToPage(1, 10);
            if (result.Items != null && result.Items.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "获取成功!";
                response.Items = result.Items;
                return response;
            }
            response.Message = "暂无数据!";
            return response;
        }
        public int GetThreeLuck(string opid)
        {
            string sql = String.Format("where WechatId='{0}' And convert(varchar(10), getdate(), 23)=convert(varchar(10), AddTime, 23) ", opid);
            List<LuckyUserList> returl = DbBase.Fetch<LuckyUserList>(sql);
            return returl.Count;
        }
        public Response Add(LuckyUserList entity)
        {
            var response = new Response();
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));
                //ThrowExceptionIfEntityIsInvalid(entity);
                var result = DbBase.Insert(entity);
                if (IsInsertSuccess(result))
                {
                    response.IsSuccess = true;
                    response.Message = "抽奖成功，星哩伴你成长哟！";
                    return response;
                }
                response.Message = "抽奖失败，请联系星哩！";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }
    }
}
