using AstWeb.Common.Helper;
using AstWeb.Common.Infrastructure;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Models
{
    [TableName("W_ArticleComments")]
    [PrimaryKey("Id")]
    public class ArticleComment : BaseEntity<int>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        [Reference(ReferenceType.OneToOne, ColumnName = "UserId", ReferenceMemberName = "Id")]
        public W_Users Users { get; set; }

        /// <summary>
        /// 文章Id
        /// </summary>
        public int ArticleId { get; set; }

        [Reference(ReferenceType.OneToOne, ColumnName = "ArticleId", ReferenceMemberName = "Id")]
        public StarCartoon StarCartoon { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int IsPraise { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否采纳当前评论
        /// </summary>
        public bool IsAdopt { get; set; }
        /// <summary>
        /// 回复的时间戳
        /// </summary>
        public long Ticks { get; set; }

        [Ignore]
        public bool IsLike { get; set; }

        /// <summary>
        /// 获取文字描述的时间
        /// </summary>
        [Ignore]
        public string TimeAgo
        {
            get
            {
                return DateHelper.TimeAgoStr(DateTime.Now, CreateTime);
            }
        }

        protected override void Validate()
        {
            if (string.IsNullOrEmpty(Content))
                AddBrokenRule(new BusinessRule(nameof(Content), "回复内容不能为空"));
        }
    }
}
