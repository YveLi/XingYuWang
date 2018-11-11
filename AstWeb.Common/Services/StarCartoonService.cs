using AstWeb.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AstWeb.Common.Infrastructure;

namespace AstWeb.Common.Services
{
    public class StarCartoonService : BaseService<StarCartoon, int>
    {
        public GetPagingResponse<StarCartoon> GetStarCartoon(int id,int pageIndex, int pageSize,string keyword)
        {
            var response = new GetPagingResponse<StarCartoon>();
            try
            {
                var result = DbBase.Query<StarCartoon>()
                    .Include(p=>p.Category)
                    .Where(p=>p.Category.Id==id && p.Title.Contains(keyword))
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
        /// <summary>
        /// 获取上一篇和下一篇
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">0:上一篇 1：下一篇</param>
        /// <returns></returns>
        public GetListsResponse<StarCartoon> GetPageArticleByOne(int id, int type = 0)
        {
            var response = new GetListsResponse<StarCartoon>();
            var result = DbBase.Query<StarCartoon>()
                .Where(p => p.IsShow == 1 && p.Id < id)
                .OrderBy(p => p.Id)
                .ThenByDescending(p => p.IsTop)
                .ThenByDescending(p => p.AddTime)
                .ThenByDescending(p => p.Sort).ToPage(1, 1);
            if (type == 1)
            {
                result = DbBase.Query<StarCartoon>()
               .Where(p => p.IsShow == 1 && p.Id > id)
               .OrderBy(p => p.Id)
               .ThenByDescending(p => p.IsTop)
               .ThenByDescending(p => p.AddTime)
               .ThenByDescending(p => p.Sort).ToPage(1, 1);
            }
            if (result.Items != null && result.Items.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "获取成功!";
                response.Items = result.Items;
                return response;
            }
            response.Message = "暂无数据!";
            return response;
        }
        public StarCartoon GetArticleById(int articleid)
        {
            return DbBase.Query<StarCartoon>().Include(p => p.User).Include(p => p.Category).SingleOrDefault(p => p.Id == articleid);
        }
        public void HitsPlusOne(StarCartoon entity)
        {
            if (entity != null)
            {
                entity.Hits++;
                DbBase.UpdateMany<StarCartoon>().OnlyFields(p => p.Hits).Where(p => p.Id == entity.Id).Execute(entity);
            }
        }
    }
}
