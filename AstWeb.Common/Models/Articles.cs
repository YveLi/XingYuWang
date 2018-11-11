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
    /// <summary>
    /// 文章
    /// </summary>
    [TableName("W_Articles")]
    [PrimaryKey("Id")]
    public class Articles : BaseEntity<int>
    {
        public string ImgUrl { get; set; }
        /// <summary>
        /// 文章主题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 编辑
        /// </summary>
        public string Editor { get; set; }
        public string Brief { get; set; }
        /// <summary>
        /// 投稿人
        /// </summary>
        public int TUserId { get; set; }

        [Reference(ReferenceType.OneToOne, ColumnName = "TUserId", ReferenceMemberName = "Id")]

        public W_Users User { get; set; }
        /// <summary>
        /// 所属分类
        /// </summary>
        public int ArticleCategoryId { get; set; }

        [Reference(ReferenceType.OneToOne, ColumnName = "ArticleCategoryId", ReferenceMemberName = "Id")]
        public ArticleCategory ArticleCategory { get; set; }
        public int ArticleParentcategoryId { get; set; }

        [Reference(ReferenceType.OneToOne, ColumnName = "ArticleParentcategoryId", ReferenceMemberName = "Id")]
        public ArticleCategory ArticleCategorys { get; set; }
        /// <summary>
        /// 悬赏积分
        /// </summary>
        public int Reward { get; set; }
        /// <summary>
        /// 文章状态
        /// </summary>
        public int ArticleStatus { get; set; }
        /// <summary>
        /// 是否为美文
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
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 最后编辑时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
        /// <summary>
        /// 点击量
        /// </summary>
        public int Hits { get; set; }
        /// <summary>
        /// 文章被收藏数
        /// </summary>
        public int Collection { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public int IsShow { get; set; }
        public string Source { get; set; }

        /// <summary>
        /// 获取文字描述的时间
        /// </summary>
        [Ignore]
        public string TimeAgo
        {
            get
            {
                return DateHelper.TimeAgoStr(DateTime.Now, AddTime);
            }
        }

        /// <summary>
        /// 获取评论数
        /// </summary>
        public int CommentCount { get; set; }

        protected override void Validate()
        {
            if (string.IsNullOrEmpty(Title) || Title.Trim().Length < 5)
                AddBrokenRule(new BusinessRule("Title", "文章标题不能少于5个字！"));
            if (Content == null || Content.Trim().Length < 20)
                AddBrokenRule(new BusinessRule("Content", "文章内容不能少于20个字！"));
            if (TUserId == 0)
                AddBrokenRule(new BusinessRule("TUserId", "请先登录！"));
            if (ArticleCategoryId == 0)
                AddBrokenRule(new BusinessRule("ArticleCategoryId", "请选择一个分类！"));
        }
    }
    public class arts
    {
        public int categoryid { get; set; }
        public string categoryname { get; set; }
        public IList<Articles> articlelist { get; set; }
    }
}
