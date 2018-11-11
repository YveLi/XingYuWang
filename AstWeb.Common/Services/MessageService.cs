using AstWeb.Common.Models;
using System;
using AstWeb.Common.Infrastructure;
using AstWeb.Common.Services.Messaging;

namespace AstWeb.Common.Services
{
    /// <summary>
    /// 消息服务类
    /// </summary>
    public class MessageService : BaseService<Message, int>
    {
        /// <summary>
        /// 根据用户ID删除消息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Response DeleteAllMessages(int userId)
        {
            var response = new Response();
            var result = DbBase.DeleteMany<Message>().Where(p => p.ToId == userId).Execute();
            if (IsUpdateSuccess(result))
            {
                response.IsSuccess = true;
                response.Message = "删除成功";
                return response;
            }
            response.Message = "删除失败";
            return response;
        }

        /// <summary>
        /// 删除一条消息
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Response DeleteOneMessage(int messageId, int userId)
        {
            var response = new Response();
            //这里传用户ID主要是验证：如果是属性他的消息才可以删除
            var result = DbBase.DeleteMany<Message>().Where(p => p.Id == messageId && p.ToId == userId).Execute();
            if (IsUpdateSuccess(result))
            {
                response.IsSuccess = true;
                response.Message = "删除成功";
                return response;
            }
            response.Message = "删除失败";
            return response;
        }
        /// <summary>
        /// 读取消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetPagingResponse<Message> GetPageMessagesByFilter(GetPageMessagesRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var response = new GetPagingResponse<Message>();
            var result = DbBase.Query<Message>()
                .Include(p => p.FormUser)
                .Where(p => p.ToId == request.UserId)
                .OrderByDescending(p => p.CreateTime)
                .ToPage(request.PageIndex, request.PageSize);

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
        /// <summary>
        /// 添加一条消息 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Response InsertMessage(Message entity)
        {
            return Insert(entity);
        }
        /// <summary>
        /// 将消息设置成已读
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public Response SetRead(int Id)
        {
            var response = new Response();

            var message = new Message
            {
                IsRead = true
            };
            var result = DbBase.UpdateMany<Message>().OnlyFields(p => p.IsRead).Where(p => p.Id == Id).Execute(message);

            if (IsUpdateSuccess(result))
            {
                response.IsSuccess = true;
                response.Message = "操作成功";
                return response;
            }

            response.Message = "操作失败";
            return response;
        }

        /// <summary>
        /// 获取未读消息的总数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetMsgCount(int userId)
        {
            return DbBase.Query<Message>().Where(p => p.ToId == userId).Count(p => !p.IsRead);
        }
    }
}
