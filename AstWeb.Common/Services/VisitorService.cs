using AstWeb.Common.Infrastructure;
using AstWeb.Common.Models;
using System;

namespace AstWeb.Common.Services
{
    /// <summary>
    /// 访客服务类
    /// </summary>
    public class VisitorService : BaseService<Visitor, int>
    {
        /// <summary>
        /// 获取最新到访的12人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public GetListsResponse<Visitor> GetTopTwelve(int userId)
        {
            var response = new GetListsResponse<Visitor>();

            var result = DbBase.Query<Visitor>()
                .Include(p => p.VisitorUser)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.LastTime)
                .ToPage(1, 12);
            if (result.Items != null && result.Items.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "获取成功";
                response.Items = result.Items;
                return response;
            }

            response.Message = "暂无访客";

            return response;
        }
        /// <summary>
        /// 获取访问总数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetVisitorNum(int userid)
        {
            return DbBase.Query<Visitor>().Include(u => u.VisitorUser).Where(u => u.UserId == userid).Count();
        }
        /// <summary>
        /// 更新访客信息
        /// </summary>
        /// <param name="entity"></param>
        public void AddVisitor(Visitor entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            ThrowExceptionIfEntityIsInvalid(entity);
            var Users = DbBase.SingleOrDefaultById<W_Users>(entity.UserId);
            if (Users != null)
            {
                Users.VisitorsNum++;
                DbBase.UpdateMany<W_Users>().OnlyFields(p => p.VisitorsNum).Where(p => p.Id == entity.UserId).Execute(Users);
            }
            var con = DbBase.Query<Visitor>().SingleOrDefault(p => p.VisitorUserId == entity.VisitorUserId && p.UserId == entity.UserId);
            if (con != null)
            {
                con.LastTime = DateTime.Now;
                DbBase.UpdateMany<Visitor>().OnlyFields(p => p.LastTime).Where(p => p.Id == con.Id).Execute(con);
                return;
            }
            DbBase.Insert(entity);
        }
    }
}
