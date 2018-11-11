using AstWeb.Common.Models;
using System;
using AstWeb.Common.Extension;
using AstWeb.Common.Infrastructure;
using AstWeb.Common.Services.Messaging;

namespace AstWeb.Common.Services
{
    public class ArticleCategoryService : BaseService<ArticleCategory, int>
    {
        public GetListsResponse<ArticleCategory> GetArticleCategories(int CategoriesId)
        {
            var response = new GetListsResponse<ArticleCategory>();
            var result = DbBase.Query<ArticleCategory>().Where(a => a.ParentId == CategoriesId).ToList();
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
        public ArticleCategory GetPostType(int CategoriesId)
        {
            return DbBase.SingleOrDefaultById<ArticleCategory>(CategoriesId);
        }

    }
}
