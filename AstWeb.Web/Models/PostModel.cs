namespace AstWeb.Models
{
    /// <summary>
    /// 帖子模型
    /// </summary>
    public class PostModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 帖子主题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 帖子内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 所属分类
        /// </summary>
        public int PostCateogryId { get; set; }
        /// <summary>
        /// 悬赏积分
        /// </summary>
        public int Reward { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Vercode { get; set; }
        /// <summary>
        /// 文章来源
        /// </summary>
        public string Source { get; set; }
    }
}