using AstWeb.Common.Infrastructure;
using AstWeb.Common.Models;

namespace AstWeb.Common.Services.Messaging
{
    public class GetPagePostsRequest : RequestBaseOfPaging
    {
        public GetPagePostsRequest(int pageIndex, int pageSize) : base(pageIndex, pageSize)
        {
        }
        /// <summary>
        /// 文章状态
        /// </summary>
        public int? ArticleStatus { get; set; }
        /// <summary>
        /// 帖子状态
        /// </summary>
        public PostStatus? PostStatus { get; set; }
        /// <summary>
        /// 是否为精贴
        /// </summary>
        public bool? IsBoutique { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int? UserId { get; set; }

        public int? CategoriesId { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public int? activityid { get; set; }
    }
}
