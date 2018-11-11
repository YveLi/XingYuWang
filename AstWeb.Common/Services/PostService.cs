using AstWeb.Common.Models;
using System;
using AstWeb.Common.Extension;
using AstWeb.Common.Infrastructure;
using AstWeb.Common.Services.Messaging;

namespace AstWeb.Common.Services
{
    /// <summary>
    /// 帖子服务类
    /// </summary>
    public class PostService : BaseService<Posts, int>
    {
        /// <summary>
        /// 帖子设置
        /// </summary>
        /// <param name="postId">帖子id</param>
        /// <param name="status">状态</param>
        /// <param name="field">设置值 置顶--加精</param>
        /// <returns></returns>
        public Response PostSet(int postId, int status, string field, int userId = 0)
        {
            var response = new Response();
            var post = DbBase.SingleOrDefaultById<Posts>(postId);
            if (post == null)
            {
                response.Message = "非法操作";
                return response;
            }

            switch (field)
            {
                //加精  -- 取消加精
                case "status":
                    post.IsBoutique = status == 1 ? 1 : 0;
                    var r1 = DbBase.UpdateMany<Posts>().OnlyFields(p => p.IsBoutique).Where(p => p.Id == postId).Execute(post);
                    var message = new Message
                    {
                        ToId = post.TUserId,
                        FormId = userId,
                        Href = "post/" + postId,
                        Content = post.Title,
                        MessageType = MessageType.Boutique,
                        CreateTime = DateTime.Now
                    };
                    DbBase.Insert(message);//添加一条消息 

                    if (IsUpdateSuccess(r1))
                    {
                        response.IsSuccess = true;
                        response.Message = "操作成功";
                        return response;
                    }
                    break;
                //置顶  -- 取消置顶
                case "stick":
                    post.IsTop = status == 1 ? 1 : 0;
                    var r2 = DbBase.UpdateMany<Posts>().OnlyFields(p => p.IsTop).Where(p => p.Id == postId).Execute(post);
                    if (IsUpdateSuccess(r2))
                    {
                        response.IsSuccess = true;
                        response.Message = "操作成功";
                        return response;
                    }
                    break;
            }
            response.Message = "操作失败";
            return response;
        }

        /// <summary>
        /// 获取最近热议Top3
        /// </summary>
        /// <returns></returns>
        public GetListsResponse<Posts> GetHotCommentPostsTopNine()
        {
            var response = new GetListsResponse<Posts>();
            var result = DbBase.Query<Posts>().OrderByDescending(p => p.CommentCount).Where(p=>p.IsShow==1).ToPage(1, 9);
            foreach(var item in result.Items)
            {
                item.User = DbBase.Query<W_Users>().Single(p => p.Id == item.TUserId);                
            }
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
        /// 根据用户id获取帖子总数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetPostCountByUserId(int userId)
        {
            int count = 0;
            if (userId == 0)
            {
                count = DbBase.Query<Posts>().Count(p => true);
            }
            else
            {
                count = DbBase.Query<Posts>().Count(p => p.TUserId == userId);
            }
            return count;
        }
        /// <summary>
        /// 检查是否有权限编辑改帖子
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="postId">帖子id</param>
        /// <returns></returns>
        public bool CheckIsEdit(int userId, int postId)
        {
            return DbBase.Query<Posts>().Any(p => p.TUserId == userId && p.Id == postId);
        }
        /// <summary>
        /// 浏览量+1
        /// </summary>
        /// <param name="entity"></param>
        public void HitsPlusOne(Posts entity)
        {
            if (entity != null)
            {
                entity.Hits++;
                DbBase.UpdateMany<Posts>().OnlyFields(p => p.Hits).Where(p => p.Id == entity.Id).Execute(entity);
            }
        }
        /// <summary>
        /// 获取单篇帖子
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public Posts GetPostById(int postId)
        {
            return DbBase.Query<Posts>().Include(p => p.User).Include(p => p.PostCategory).SingleOrDefault(p => p.Id == postId);
        }
        /// <summary>
        /// 获取帖子列表
        /// </summary>
        /// <param name="request">搜索条件</param>
        /// <returns></returns>
        public GetPagingResponse<Posts> GetPagePostsByFilter(GetPagePostsRequest request)
        {
            var response = new GetPagingResponse<Posts>();
            var result = DbBase.Query<Posts>()
                .Include(p => p.User) //联表查询
                .Include(p => p.PostCategory)
                .HasWhere(request.IsBoutique, p => p.IsBoutique == 1)
                .HasWhere(request.PostStatus, p => p.PostStatus == request.PostStatus)
                .HasWhere(request.UserId, p => p.TUserId == request.UserId)
                .Where(p => p.IsShow == 1) //查询状态为显示的数据
                .OrderByDescending(p => p.IsTop)  //排序方式：倒序，顺序：是否置顶->排序值->创建时间
                .ThenByDescending(p => p.CreateTime)
                .ThenByDescending(p => p.Sort)
                .ToPage(request.PageIndex, request.PageSize);
            if (request.CategoriesId != 0 && request.CategoriesId != null)
            {
                result = DbBase.Query<Posts>()
                .Include(p => p.User) //联表查询
                .Include(p => p.PostCategory)
                .HasWhere(request.IsBoutique, p => p.IsBoutique == 1)
                .HasWhere(request.PostStatus, p => p.PostStatus == request.PostStatus)
                .HasWhere(request.UserId, p => p.TUserId == request.UserId)
                .Where(p => p.IsShow == 1 && p.PostCategoryId == request.CategoriesId) //查询状态为显示的数据
                .OrderByDescending(p => p.IsTop)  //排序方式：倒序，顺序：是否置顶->排序值->创建时间
                .ThenByDescending(p => p.Sort)
                .ThenByDescending(p => p.CreateTime)
                .ToPage(request.PageIndex, request.PageSize);
            }
            if (request.Title != null)
            {
                result = DbBase.Query<Posts>()
                .Include(p => p.User) //联表查询
                .Include(p => p.PostCategory)
                .HasWhere(request.IsBoutique, p => p.IsBoutique == 1)
                .HasWhere(request.PostStatus, p => p.PostStatus == request.PostStatus)
                .HasWhere(request.UserId, p => p.TUserId == request.UserId)
                .Where(p => p.IsShow == 1 && p.Title.Contains(request.Title)) //查询状态为显示的数据
                .OrderByDescending(p => p.IsTop)  //排序方式：倒序，顺序：是否置顶->排序值->创建时间
                .ThenByDescending(p => p.Sort)
                .ThenByDescending(p => p.CreateTime)
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
        /// 发表帖子
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public Response SendPost(Posts entity)
        {
            return InsertOrUpdate(entity, true);
        }
        /// <summary>
        /// 更新帖子信息
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public Response UpdatePost(Posts entity)
        {
            return InsertOrUpdate(entity, false);
        }
        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Response DeletePost(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var response = new Response();

            using (var tran = DbBase.GetTransaction())
            {
                //删除帖子
                var r1 = DbBase.Delete<Posts>(id);
                //删除评论
                var r2 = DbBase.DeleteMany<Comment>().Where(p => p.PostId == id).Execute();

                tran.Complete();
                if (IsUpdateSuccess(r1))
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
        /// 获取最近热贴Top15
        /// </summary>
        /// <returns></returns>
        public GetListsResponse<Posts> GetHotPostsTopFifteen()
        {
            var response = new GetListsResponse<Posts>();

            var result = DbBase.Query<Posts>().OrderByDescending(p => p.Hits).ToPage(1, 15);
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


        //新增或修改
        private Response InsertOrUpdate(Posts entity, bool isInsert)
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
                if (user.Integral < entity.Reward)
                {
                    response.Message = "星币余额不足，发布失败！";
                    return response;
                }
                using (var tran = DbBase.GetTransaction())
                {
                    user.Integral -= entity.Reward;
                    DbBase.UpdateMany<W_Users>().OnlyFields(p => p.Integral).Where(p => p.Id == entity.TUserId).Execute(user);//只更新积分
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
                response.Message = "发布成功";
                return response;
            }
            response.Message = "发布失败!";
            return response;
        }
        public int GetPostsCount(int CategoriesId)
        {
            int count = 0;
            if (CategoriesId == 0)
            {
                count = DbBase.Query<Posts>().Count(p => true);
            }
            else
            {
                count = DbBase.Query<Posts>().Count(p => p.PostCategoryId == CategoriesId);
            }
            return count;
        }
        //帖子加精或取消加精
        public int Boutique(int id)
        {
            var entity = DbBase.Query<Posts>().SingleOrDefault(p => p.Id == id);
            if(entity.IsBoutique == 1)
            {
                entity.IsBoutique = 0;
            }
            else
            {
                entity.IsBoutique = 1;
            }
            var response = DbBase.UpdateMany<Posts>().OnlyFields(p => p.IsBoutique).Where(p => p.Id == entity.Id).Execute(entity);
            return response;
        }
        //帖子置顶或取消置顶
        public int Top(int id)
        {
            var entity = DbBase.Query<Posts>().SingleOrDefault(p => p.Id == id);
            if (entity.IsTop == 1)
            {
                entity.IsTop = 0;
            }
            else
            {
                entity.IsTop = 1;
            }
            var response = DbBase.UpdateMany<Posts>().OnlyFields(p => p.IsTop).Where(p => p.Id == entity.Id).Execute(entity);
            return response;
        }
        //帖子隐藏
        public int Show(int id)
        {
            var entity = DbBase.Query<Posts>().SingleOrDefault(p => p.Id == id);
            entity.IsShow = 0;
            var response = DbBase.UpdateMany<Posts>().OnlyFields(p => p.IsShow).Where(p => p.Id == entity.Id).Execute(entity);
            return response;
        }
    }
}
