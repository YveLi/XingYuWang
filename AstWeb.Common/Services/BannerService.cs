using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AstWeb.Common.Infrastructure;
using AstWeb.Common.Models;

namespace AstWeb.Common.Services
{
    public class BannerService : BaseService<BannerList, int>
    {
        /// <summary>
        /// 获取banner列表
        /// </summary>
        /// <param name="type">0：index_banner 1：index_news_banner 2:星座页news_banner</param>
        /// <returns></returns>
        public GetListsResponse<BannerList> GetBannerList(int type = 0)
        {
            var response = new GetListsResponse<BannerList>();
            var result = DbBase.Query<BannerList>().OrderByDescending(b => b.Sort).Where(b => b.ModuleType == type).ToList();
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
