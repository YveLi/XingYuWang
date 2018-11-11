using AstWeb.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AstWeb.Common.Infrastructure;

namespace AstWeb.Common.Services
{
    public class CartoonCategoriesService : BaseService<CartoonCategories, int>
    {
        public GetPagingResponse<CartoonCategories> GetPageCartoonCategories(int pageIndex, int pageSize,string keyword)
        {
            var response = new GetPagingResponse<CartoonCategories>();
            try
            {
                var result = DbBase.Query<CartoonCategories>()
               .Where(p=>p.Name.Contains(keyword) || p.Memo.Contains(keyword))
               .ToPage(pageIndex, pageSize);
                if (result.Items != null && result.Items.Count > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "获取成功";
                    response.Pages = result;
                    return response;
                }
                response.Message = "暂无数据";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
