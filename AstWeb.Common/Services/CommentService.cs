using AstWeb.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AstWeb.Common.Infrastructure;

namespace AstWeb.Common.Services
{
    public class CommentService : BaseService<Comment, int>
    {
        /// <summary>
        /// 添加或减少赞
        /// </summary>
        /// <param name="commentId">评论Id</param>
        /// <param name="ok"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Response AddOrRemoveLike(int commentId, bool ok, int userId)
        {
            var response = new Response();
            var comment = DbBase.SingleOrDefaultById<Comment>(commentId);
            if (comment == null)
            {
                response.Message = "非法操作";
                return response;
            }
            using (var tran = DbBase.GetTransaction())
            {
                if (ok)
                {
                    comment.IsPraise--;
                    DbBase.DeleteMany<ArticleLikes>().Where(p => p.CommentId == commentId && p.UserId == userId).Execute();
                }
                else
                {
                    comment.IsPraise++;
                    DbBase.Insert(new ArticleLikes
                    {
                        CommentId = commentId,
                        UserId = userId
                    });
                }
                var result = DbBase.UpdateMany<Comment>().OnlyFields(p => p.IsPraise).Where(p => p.Id == commentId).Execute(comment);
                tran.Complete();
                if (IsUpdateSuccess(result))
                {
                    response.IsSuccess = true;
                    response.Message = "操作成功";
                    return response;
                }
            }
            response.Message = "操作失败";

            return response;
        }

        /// <summary>
        /// 根据帖子ID获取评论数
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public int GetCommentCountByPostId(int postId)
        {
            return DbBase.Query<Comment>().Count(p => p.PostId == postId);
        }

        /// <summary>
        /// 获取评论信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public GetPagingResponse<Comment> GetPageCommentsByPostId(int pageIndex, int pageSize, int postId)
        {
            var response = new GetPagingResponse<Comment>();
            try
            {
                var result = DbBase.Query<Comment>().Include(p => p.Users)
               .Where(p => p.PostId == postId)
               .OrderBy(p => p.Id)
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
        public Response AddComment(Comment entity, int currentUserId, IList<string> names = null)
        {
            var post = DbBase.SingleOrDefaultById<Posts>(entity.PostId);//获取帖子信息 

            //如果不是自己回复自己，才添加一条消息
            if (post.TUserId != currentUserId)
            {
                //贡献榜+1
                new ContributionService().AddContribution(new Contribution
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
                    Href = "/social/post/detail/" + entity.PostId + "page_1" +"#articlecoment",
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
                                Href = "post/" + entity.PostId + "#item_" + entity.Ticks,
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
            DbBase.UpdateMany<Posts>().OnlyFields(p => p.CommentCount).Where(p => p.Id == entity.PostId).Execute(post);


            return Insert(entity);
        }

        /// <summary>
        /// 帖子采纳
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public Response Accept(int userId, int commentId)
        {
            var response = new Response();
            var comment = DbBase.SingleOrDefaultById<Comment>(commentId);
            if (comment == null)
            {
                response.Message = "操作失败";
                return response;
            }
            var ispost = DbBase.Query<Posts>().Any(p => p.Id == comment.PostId && p.TUserId == userId);
            if (!ispost)
            {
                response.Message = "操作失败";
                return response;
            }
            //修改帖子状态
            DbBase.UpdateMany<Posts>().OnlyFields(p => p.PostStatus).Where(p => p.Id == comment.PostId).Execute(new Posts
            {
                PostStatus = PostStatus.Close
            });
            comment.IsAdopt = true;
            var result = DbBase.UpdateMany<Comment>().OnlyFields(p => p.IsAdopt).Where(p => p.Id == commentId).Execute(comment);
            //给予奖励
            var Post = DbBase.SingleOrDefaultById<Posts>(comment.PostId);
            var commer = DbBase.SingleOrDefaultById<W_Users>(comment.UserId);
            DbBase.UpdateMany<W_Users>().OnlyFields(p => p.Integral).Where(p => p.Id == comment.UserId).Execute(new W_Users
            {
                Integral = commer.Integral + Post.Reward
            });
            new PostContributionService().AddContribution(new PostContribution
            {
                UserId = comment.UserId,
                Time = DateTime.Now.ToString("yyyy-MM"),
                Number = Post.Reward,
                UpdateTime = DateTime.Now,
                PostId = Post.Id
            });
            if (result > 0)
            {
                response.IsSuccess = true;
                response.Message = "操作成功";
                return response;
            }

            response.Message = "操作失败";

            return response;
        }

        /// <summary>
        /// 根据用户Id获取评论信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public GetPagingResponse<Comment> GetPageCommentsByUserId(int pageIndex, int pageSize, int userId)
        {
            var response = new GetPagingResponse<Comment>();

            var result = DbBase.Query<Comment>().Include(p => p.Users).Include(p => p.Posts)
                .Where(p => p.UserId == userId)
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

            return response;
        }
    }
}
