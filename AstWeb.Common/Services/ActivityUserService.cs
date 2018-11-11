using AstWeb.Common.Models;
using System;
using AstWeb.Common.Extension;
using AstWeb.Common.Infrastructure;
using AstWeb.Common.Services.Messaging;
using System.Collections.Generic;
using AstWeb.Common.Helper;

namespace AstWeb.Common.Services
{
    public class ActivityUserService : BaseService<ActivityUser, int>
    {
        public GetPagingResponse<ActivityUser> GetActivityUser(GetPagePostsRequest request)
        {
            var response = new GetPagingResponse<ActivityUser>();
            var result = DbBase.Query<ActivityUser>().Where(p => p.ActivityId == request.activityid) //查询状态为显示的数据
              .OrderByDescending(p => p.voteNum)  //排序方式：倒序，顺序：是否置顶->排序值->创建时间
              .ThenByDescending(p => p.AddTime)
              .ToPage(request.PageIndex, request.PageSize);
            if (request.Title != "" && request.Title != null)
            {
                result = DbBase.Query<ActivityUser>().Where(p => p.Name.Contains(request.Title) || p.Class.Contains(request.Title) && p.ActivityId == request.activityid) //查询状态为显示的数据
              .OrderByDescending(p => p.voteNum)  //排序方式：倒序，顺序：是否置顶->排序值->创建时间
              .ThenByDescending(p => p.AddTime)
                  .ToPage(request.PageIndex, request.PageSize);
            }
            if (DataConversion.StrToInt(request.Title) > 0)
            {
                result = DbBase.Query<ActivityUser>().Where(p => p.Id == DataConversion.StrToInt(request.Title) && p.ActivityId == request.activityid) //查询状态为显示的数据
            .OrderByDescending(p => p.voteNum)  //排序方式：倒序，顺序：是否置顶->排序值->创建时间
            .ThenByDescending(p => p.AddTime)
                .ToPage(request.PageIndex, request.PageSize);
            }
            if (result.Items != null && result.Items.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "获取成功!";
                response.Pages = result;
                return response;
            }
            response.Message = "暂无数据!";
            return response;
        }

        public void VotePlusOne(ActivityUser entity)
        {
            if (entity != null)
            {
                entity.voteNum++;
                DbBase.UpdateMany<ActivityUser>().OnlyFields(p => p.voteNum).Where(p => p.Id == entity.Id).Execute(entity);
            }
        }
        public GetListsResponse<ActivityUser> GetActivityTop(int activityid)
        {
            var response = new GetListsResponse<ActivityUser>();
            var result = DbBase.Query<ActivityUser>().Where(o => o.ActivityId == activityid).OrderByDescending(p => p.voteNum).ToPage(1, 100);
            if (result.Items != null && result.Items.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "获取成功";
                response.Items = result.Items;
                return response;
            }
            response.Message = "暂无数据";
            return response;
        }
        public int GetUserCount(int activityid)
        {
            var result = DbBase.Query<ActivityUser>().Where(o => o.ActivityId == activityid).Count();
            return result;
        }
        public Response Add(ActivityUser entity)
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
                    response.Message = "报名成功！";
                    return response;
                }
                response.Message = "报名失败，请检查之后重新报名！";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public ActivityUser GetActivtyUserMd(int id)
        {
            return DbBase.SingleOrDefaultById<ActivityUser>(id);
        }

        public ActivityUser GetModel(string tel)
        {
            return DbBase.Query<ActivityUser>().Where(o => o.Mobile == tel).SingleOrDefault();
        }
    }
}
