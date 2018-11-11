using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AstWeb.Common.Infrastructure;
using AstWeb.Common.Models;

namespace AstWeb.Common.Services
{
    public class ActivityBannerListService : BaseService<ActivityBannerList, int>
    {
        public GetListsResponse<ActivityBannerList> GetBannerList(int type = 0)
        {
            var response = new GetListsResponse<ActivityBannerList>();
            var result = DbBase.Query<ActivityBannerList>().OrderByDescending(b => b.Sort).ToList();
            if (result != null && result.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "获取成功";
                response.Items = result;
                return response;
            }
            response.Message = "暂无数据";
            return response;
        }
    }
}
