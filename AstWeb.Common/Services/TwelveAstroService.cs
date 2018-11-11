using AstWeb.Common.Infrastructure;
using AstWeb.Common.Models;
using AstWeb.Common.Services.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Services
{
    public class TwelveAstroService : BaseService<Posts, int>
    {
        public GetListsResponse<TwelveAstro> GetTwelveAstroList()
        {
            var response = new GetListsResponse<TwelveAstro>();
            var result = DbBase.Query<TwelveAstro>()
                .OrderBy(p => p.Id).ToList();  //排序方式：倒序，顺序：是否置顶->排序值->创建时间
            if (result.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "获取成功!";
                response.Items = result;
                return response;
            }
            response.Message = "暂无数据!";
            return response;
        }
    }
}
