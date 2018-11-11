using AstWeb.Common.Infrastructure;
using System;
using NPoco;
using AstWeb.Common.Helper;

namespace AstWeb.Common.Models
{
    /// <summary>
    /// 贴子评论实体
    /// </summary>
    [TableName("W_Comments")]
    [PrimaryKey("Id")]
    public class Comment : BaseEntity<int>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        [Reference(ReferenceType.OneToOne, ColumnName = "UserId", ReferenceMemberName = "Id")]
        public W_Users Users { get; set; }
        /// <summary>
        /// 帖子Id
        /// </summary>
        public int PostId { get; set; }

        [Reference(ReferenceType.OneToOne, ColumnName = "PostId", ReferenceMemberName = "Id")]
        public Posts Posts { get; set; }
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
