using AstWeb.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AstWeb.Common.Infrastructure;

namespace AstWeb.Common.Services
{
    public class ArticleCommentService : BaseService<ArticleComment, int>
    {
        /// <summary>
        /// 获取评论信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public GetPagingResponse<ArticleComment> GetPageCommentsByArticleId(int pageIndex, int pageSize, int articleid)
        {
            var response = new GetPagingResponse<ArticleComment>();
            try
            {
                var result = DbBase.Query<ArticleComment>().Include(p => p.Users)
               .Where(p => p.ArticleId == articleid)
               .OrderByDescending(p => p.CreateTime)
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
        /// 评论
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Response AddComment(ArticleComment entity, int currentUserId, IList<string> names = null)
        {
            var post = DbBase.SingleOrDefaultById<Articles>(entity.ArticleId);//获取帖子信息 
            //如果不是自己回复自己，才添加一条消息
            if (post.TUserId != currentUserId)
            {
                //贡献榜+1
                new ArticleContributionService().AddContribution(new ArticleContribution
                {
                    UserId = currentUserId,
                    Time = DateTime.Now.ToString("yyyy-MM"),
                    Number = 1,
                    UpdateTime = DateTime.Now
                });
                var message = new Message
                {
                    ToId = post.TUserId,
                    FormId = currentUserId,
                    Href = "/astro/detail/" + entity.ArticleId+ "#articlecoment",
                    Content = post.Title,
                    MessageType = MessageType.Answer,
                    CreateTime = DateTime.Now
                };
                DbBase.Insert(message);//添加一条消息 
            }
            //处理通知消息
            if (names != null && names.Count > 0)
            {
                var where = new StringBuilder();
                var args = new object[names.Count];
                where.Append(" WHERE");
                for (int i = 0; i < names.Count; i++)
                {
                    if (i == (names.Count - 1))
                        where.Append(" Nickname=@" + i);
                    else
                        where.Append(" Nickname=@" + i + " OR");

                    args[i] = names[i];
                }
                var users = DbBase.Fetch<W_Users>(where.ToString(), args);
                if (users != null && users.Count > 0)
                {
                    foreach (var item in users)
                    {
                        if (item.Id != currentUserId)
                        {
                            var message = new Message
                            {
                                ToId = item.Id,
                                FormId = currentUserId,
                                Href = "article/" + entity.ArticleId + "#item_" + entity.Ticks,
                                Content = post.Title,
                                MessageType = MessageType.Reply,
                                CreateTime = DateTime.Now
                            };
                            DbBase.Insert(message);//添加一条消息 
                        }
                    }
                }
            }
            //帖子回复人数+1
            post.CommentCount++;
            //更新评论人数
            DbBase.UpdateMany<Articles>().OnlyFields(p => p.CommentCount).Where(p => p.Id == entity.ArticleId).Execute(post);
            return Insert(entity);
        }
    }
}
