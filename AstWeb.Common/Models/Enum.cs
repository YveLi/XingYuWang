using System.ComponentModel;

namespace AstWeb.Common.Models
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender
    {
        [Description("男")]
        Man = 1,
        [Description("女")]
        Woman = 0
    }
    /// <summary>
    /// 帖子状态
    /// </summary>
    public enum PostStatus
    {
        /// <summary>
        /// 未解决
        /// </summary>
        Open = 1,
        /// <summary>
        /// 已解决
        /// </summary>
        Close = 2
    }
    /// <summary>
    /// 文章状态
    /// </summary>
    public enum StarCartoontatus
    {
        /// <summary>
        /// 未审核
        /// </summary>
        Close = 0,
        /// <summary>
        /// 已审核
        /// </summary>
        Open = 1
        /// <summary>
    }
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 回复
        /// </summary>
        Reply = 1,
        /// <summary>
        /// 解答
        /// </summary>
        Answer = 2,
        /// <summary>
        /// 帖子被加精通知
        /// </summary>
        Boutique = 3
    }
}
