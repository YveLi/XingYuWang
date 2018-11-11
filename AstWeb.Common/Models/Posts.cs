using AstWeb.Common.Helper;
using AstWeb.Common.Infrastructure;
using NPoco;
using System;

namespace AstWeb.Common.Models
{
    /// <summary>
    /// 帖子实体
    /// </summary>
    [TableName("W_Posts")]
    [PrimaryKey("Id")]
    public class Posts : BaseEntity<int>
    {
        /// <summary>
        /// 帖子主题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 帖子内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 发表人
        /// </summary>
        public int TUserId { get; set; }

        [Reference(ReferenceType.OneToOne, ColumnName = "TUserId", ReferenceMemberName = "Id")]
        public W_Users User { get; set; }
        /// <summary>
        /// 所属分类
        /// </summary>
        public int PostCategoryId { get; set; }

        [Reference(ReferenceType.OneToOne, ColumnName = "PostCategoryId", ReferenceMemberName = "Id")]
        public PostCategory PostCategory { get; set; }
        /// <summary>
        /// 悬赏积分
        /// </summary>
        public int Reward { get; set; }
        /// <summary>
        /// 帖子状态
        /// </summary>
        public PostStatus PostStatus { get; set; }
        /// <summary>
        /// 是否为精贴
        /// </summary>
        public int IsBoutique { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public int IsTop { get; set; }
        /// <summary>
        /// 排序值
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 发表时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后编辑时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 点击量
        /// </summary>
        public int Hits { get; set; }
        /// <summary>
        /// 帖子被收藏数
        /// </summary>
        public int Collection { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public int IsShow { get; set; }

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

        public string KeyWords { get; set; }

        public string Description { get; set; }
        /// <summary>
        /// 获取评论数
        /// </summary>
        public int CommentCount { get; set; }

        protected override void Validate()
        {
            if (string.IsNullOrEmpty(Title) || Title.Trim().Length < 5)
                AddBrokenRule(new BusinessRule("Title", "帖子标题不能少于10个字！"));
            if (Content == null || Content.Trim().Length < 20)
                AddBrokenRule(new BusinessRule("Content", "帖子内容不能少于20个字！"));
            if (TUserId == 0)
                AddBrokenRule(new BusinessRule("UserId", "请先登录！"));
            if (PostCategoryId == 0)
                AddBrokenRule(new BusinessRule("PostCateogryId", "请选择一个分类！"));
        }
    }
}
