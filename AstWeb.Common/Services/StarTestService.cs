using AstWeb.Common.Infrastructure;
using AstWeb.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Services
{
    public class StarTestService : BaseService<StarTest, int>
    {
        /// <summary>
        /// 获取测试列表
        /// </summary>
        /// <param name="request">搜索条件</param>
        /// <returns></returns>
        public GetListsResponse<StarTest> GetTest()
        {
            var response = new GetListsResponse<StarTest>();
             var result = DbBase.Query<StarTest>()
                .Where(p => p.Id > 0)
                .ToPage(1, 18); ;
            if (result.Items != null && result.Items.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "获取成功!";
                response.Items = result.Items;
                return response;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "获取失败!";
                return response;
            }
        }
        public StarTest GetTestById(int id)
        {
            return DbBase.Query<StarTest>().SingleOrDefault(p => p.Id == id);
        }
    }
}
