using AstWeb.Common.Infrastructure;
using NPoco;

namespace AstWeb.Common.Models
{
    [TableName("W_ArticleLikes")]
    [PrimaryKey("Id")]
    public class ArticleLikes : BaseEntity<int>
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public int CommentId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        protected override void Validate()
        {
            //throw new NotImplementedException();
        }
    }
}
