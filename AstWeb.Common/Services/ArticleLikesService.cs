using AstWeb.Common.Models;

namespace AstWeb.Common.Services
{
    /// <summary>
    /// zan
    /// </summary>
    public class ArticleLikesService : BaseService<ArticleLikes, int>
    {
        /// <summary>
        /// 是否已赞
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool ZanIsExists(int commentId, int userId)
        {
            return DbBase.Query<ArticleLikes>().Any(p => p.CommentId == commentId && p.UserId == userId);
        }
    }
}
