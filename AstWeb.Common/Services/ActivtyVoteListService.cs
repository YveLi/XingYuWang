using AstWeb.Common.Models;
using System;
using AstWeb.Common.Extension;
using AstWeb.Common.Infrastructure;
using AstWeb.Common.Services.Messaging;
using System.Collections.Generic;

namespace AstWeb.Common.Services
{
    public class ActivtyVoteListService : BaseService<ActivtyList, int>
    {
        public int GetVoteCount(int activityid)
        {
            var count = 0;
            IList<ActivityUser> userlist = DbBase.Query<ActivityUser>().Where(o => o.ActivityId == activityid).ToList();
            foreach (var item in userlist)
            {
                count += item.voteNum;
            }
            return count;
        }

        public bool GetIsVote(string opid, int userid)
        {
            string sql = String.Format("where WechatId='{0}' And convert(varchar(10), getdate(), 23)=convert(varchar(10), AddTime, 23) AND PlayerId={1} ", opid, userid);
            List<ActivtyVoteList> returl = DbBase.Fetch<ActivtyVoteList>(sql);
            return returl.Count > 0;
        }

        public int GetThreeVote(string opid)
        {
            string sql = String.Format("where WechatId='{0}' And convert(varchar(10), getdate(), 23)=convert(varchar(10), AddTime, 23) ", opid);
            List<ActivtyVoteList> returl = DbBase.Fetch<ActivtyVoteList>(sql);
            return returl.Count;
        }
        public Response Add(ActivtyVoteList entity)
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
                    response.Message = "投票成功，星哩伴你成长哟！";
                    return response;
                }
                response.Message = "投票失败，请联系星哩！";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }
    }
}
