using AstWeb.Common.Models;
using System;
using AstWeb.Common.Extension;
using AstWeb.Common.Infrastructure;
using AstWeb.Common.Services.Messaging;
using System.Collections.Generic;
using System.Linq;

namespace AstWeb.Common.Services
{
    public class ArticlesService : BaseService<Articles, int>
    {
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="request">搜索条件</param>
        /// <returns></returns>
        public GetPagingResponse<Articles> GetPageArticlesByFilter(GetPagePostsRequest request)
        {
            var response = new GetPagingResponse<Articles>();
            var result = DbBase.Query<Articles>()
                .Include(p => p.User) //联表查询
                .Include(p => p.ArticleCategory)
                .Include(p => p.ArticleCategorys)
                .HasWhere(request.IsBoutique, p => p.IsBoutique == 1)
                .HasWhere(request.PostStatus, p => p.ArticleStatus == request.ArticleStatus)
                .HasWhere(request.UserId, p => p.TUserId == request.UserId)
                .Where(p => p.IsShow == 1) //查询状态为显示的数据
                .OrderByDescending(p => p.IsTop)  //排序方式：倒序，顺序：是否置顶->排序值->创建时间
                .ThenByDescending(p => p.AddTime)
                .ThenByDescending(p => p.Sort)
                .ToPage(request.PageIndex, request.PageSize);
            if (request.CategoriesId != 0 && request.CategoriesId != null)
            {
                result = DbBase.Query<Articles>()
                .Include(p => p.User) //联表查询
                .Include(p => p.ArticleCategory)
                .Include(p => p.ArticleCategorys)
                .HasWhere(request.IsBoutique, p => p.IsBoutique == 1)
                .HasWhere(request.PostStatus, p => p.ArticleStatus == request.ArticleStatus)
                .HasWhere(request.UserId, p => p.TUserId == request.UserId)
                .Where(p => p.IsShow == 1 && p.ArticleCategoryId == request.CategoriesId) //查询状态为显示的数据
                .OrderByDescending(p => p.IsTop)  //排序方式：倒序，顺序：是否置顶->排序值->创建时间
                .ThenByDescending(p => p.Sort)
                .ThenByDescending(p => p.AddTime)
                .ToPage(request.PageIndex, request.PageSize);
            }
            if (!String.IsNullOrEmpty(request.Title))
            {
                result = DbBase.Query<Articles>()
                .Include(p => p.User) //联表查询
                .Include(p => p.ArticleCategory)
                .Include(p => p.ArticleCategorys)
                .HasWhere(request.IsBoutique, p => p.IsBoutique == 1)
                .HasWhere(request.PostStatus, p => p.ArticleStatus == request.ArticleStatus)
                .HasWhere(request.UserId, p => p.TUserId == request.UserId)
                .Where(p => p.IsShow == 1 && p.Title.Contains(request.Title)) //查询状态为显示的数据
                .OrderByDescending(p => p.IsTop)  //排序方式：倒序，顺序：是否置顶->排序值->创建时间
                .ThenByDescending(p => p.Sort)
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
        /// <summary>
        /// 获取上一篇和下一篇
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">0:上一篇 1：下一篇</param>
        /// <returns></returns>
        public GetListsResponse<Articles> GetPageArticleByOne(int id, int type = 0)
        {
            var response = new GetListsResponse<Articles>();
            var result = DbBase.Query<Articles>()
                .Where(p => p.IsShow == 1 && p.Id < id)
                .OrderBy(p => p.Id)
                .ThenByDescending(p => p.IsTop)
                .ThenByDescending(p => p.AddTime)
                .ThenByDescending(p => p.Sort).ToPage(1, 1);
            if (type == 1)
            {
                result = DbBase.Query<Articles>()
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
        /// <summary>
        /// 热门文章
        /// </summary>
        /// <returns></returns>
        public GetListsResponse<Articles> GetHotArticlesTopTen()
        {
            var response = new GetListsResponse<Articles>();
            var result = DbBase.Query<Articles>().OrderByDescending(p => p.Hits).ToPage(1, 10);
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
        public GetListsResponse<arts> GetArticlesToType()
        {
            var response = new GetListsResponse<arts>();
            var typelist = DbBase.Query<ArticleCategory>().Where(ac => ac.ParentId == 0).ToList();
            IList<arts> artslist = new List<arts>();
            foreach (var item in typelist)
            {
                var result = DbBase.Query<Articles>().Include(p => p.ArticleCategory)
                .Include(p => p.ArticleCategorys).OrderByDescending(p => p.Hits).Where(ac => ac.ArticleParentcategoryId == item.Id).ToPage(1, 2);
                artslist.Add(new arts
                {
                    categoryid = item.Id,
                    categoryname = item.CategoryName,
                    articlelist = result.Items
                });
            }
            if (artslist != null && artslist.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "获取成功";
                response.Items = artslist;
                return response;
            }
            response.Message = "暂无数据";
            return response;
        }
        /// <summary>
        /// 星座快讯(随机获取6条)
        /// </summary>
        /// <returns></returns>
        public GetListsResponse<Articles> GetFastArticles()
        {
            var response = new GetListsResponse<Articles>();
            var result = DbBase.Query<Articles>().Where(p => p.IsBoutique == 1).OrderByDescending(p => p.Hits).ToPage(1, 6);
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

        /// <summary>
        /// 获取随机10篇文章
        /// </summary>
        /// <returns></returns>
        public GetListsResponse<Articles> GetRandomArticles()
        {
            var response = new GetListsResponse<Articles>();
            var result = DbBase.Query<Articles>().Include(p => p.ArticleCategory)
                .Include(p => p.ArticleCategorys).OrderByDescending(p => p.CommentCount).ToPage(1, 10);
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
        public void HitsPlusOne(Articles entity)
        {
            if (entity != null)
            {
                entity.Hits++;
                DbBase.UpdateMany<Articles>().OnlyFields(p => p.Hits).Where(p => p.Id == entity.Id).Execute(entity);
            }
        }
        public Articles GetArticleById(int articleid)
        {
            return DbBase.Query<Articles>().Include(p => p.User).Include(p => p.ArticleCategory).Include(p => p.ArticleCategorys).SingleOrDefault(p => p.Id == articleid);
        }
        public GetListsResponse<Articles> GetArticlesByUserId(int userid)
        {
            var response = new GetListsResponse<Articles>();
            var result = DbBase.Query<Articles>().Where(p => p.TUserId == userid).ToPage(1, 10);
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
        public Response AddArticle(Articles entity)
        {
            return InsertOrUpdate(entity, true);
        }
        /// <summary>
        /// 更新帖子信息
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public Response UpdateArticle(Articles entity)
        {
            return InsertOrUpdate(entity, false);
        }
        private Response InsertOrUpdate(Articles entity, bool isInsert)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            var response = new Response();
            ThrowExceptionIfEntityIsInvalid(entity);//验证数据的正确性，入库的最后一道关卡
            bool flag = false;
            if (isInsert)
            {
                var user = DbBase.SingleOrDefaultById<W_Users>(entity.TUserId);
                if (user == null)
                {
                    response.Message = "非法操作，请重新登录重试！";
                    return response;
                }
                //if (user.Integral < entity.Reward)
                //{
                //    response.Message = "星币余额不足，发布失败！";
                //    return response;
                //}
                using (var tran = DbBase.GetTransaction())
                {
                    //user.Integral -= entity.Reward;
                    //DbBase.UpdateMany<W_Users>().OnlyFields(p => p.Integral).Where(p => p.Id == entity.TUserId).Execute(user);//只更新积分
                    var result = DbBase.Insert(entity);
                    flag = result != null && Convert.ToInt32(result) > 0;
                    tran.Complete();
                }
            }
            else
            {
                var result = DbBase.Update(entity);
                flag = result > 0;
            }
            if (flag)
            {
                response.IsSuccess = true;
                response.Message = "投稿成功,请耐心等待审核";
                return response;
            }
            response.Message = "投稿失败!";
            return response;
        }
    }
}
