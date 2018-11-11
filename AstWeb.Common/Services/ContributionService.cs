using AstWeb.Common.Models;
using System;
using AstWeb.Common.Infrastructure;

namespace AstWeb.Common.Services
{

    public class ContributionService : BaseService<Contribution, int>
    {

        /// <summary>
        /// 获取月贡献TOP12
        /// </summary>
        /// <returns></returns>
        public GetListsResponse<Contribution> GetTopTwelve()
        {
            var response = new GetListsResponse<Contribution>();

            var result = DbBase.Query<Contribution>()
                .Include(p => p.User)
                .Where(p => p.Time == DateTime.Now.ToString("yyyy-MM"))
                .OrderByDescending(p => p.Number)
                .ToPage(1, 12);
            if (result.Items != null && result.Items.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "获取成功";
                response.Items = result.Items;
                return response;
            }

            response.Message = "获取失败";

            return response;
        }
        //贡献+1
        public void AddContribution(Contribution entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            ThrowExceptionIfEntityIsInvalid(entity);
            try
            {
                var con = DbBase.Query<Contribution>().SingleOrDefault(p => p.UserId == entity.UserId && p.Time == DateTime.Now.ToString("yyyy-MM"));
                if (con != null)
                {
                    con.Number++;
                    DbBase.UpdateMany<Contribution>().OnlyFields(p => p.Number).Where(p => p.Id == con.Id).Execute(con);
                    return;
                }
                DbBase.Insert(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
