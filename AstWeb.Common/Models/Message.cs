using AstWeb.Common.Helper;
using AstWeb.Common.Infrastructure;
using NPoco;
using System;

namespace AstWeb.Common.Models
{

    /// <summary>
    /// 消息实体。
    /// </summary>
    [TableName("W_Message")]
    [PrimaryKey("Id")]
    public class Message : BaseEntity<int>
    {
        /// <summary>
        /// 发送者
        /// </summary>
        public int FormId { get; set; }

        [Reference(ReferenceType.OneToOne, ColumnName = "FormId", ReferenceMemberName = "Id")]
        public W_Users FormUser { get; set; }
        /// <summary>
        /// 接收者
        /// </summary>
        public int ToId { get; set; }
        [Reference(ReferenceType.OneToOne, ColumnName = "ToId", ReferenceMemberName = "Id")]
        public W_Users ToUser { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Href { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// 时间文字描述
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
            if (FormId == 0)
                AddBrokenRule(new BusinessRule(nameof(FormId), "消息发送者不能为空！"));
            if (ToId == 0)
                AddBrokenRule(new BusinessRule(nameof(ToId), "消息接受者不能为空！"));
            if (string.IsNullOrEmpty(Href))
                AddBrokenRule(new BusinessRule(nameof(Href), "链接地址不能为空！"));
            if (string.IsNullOrEmpty(Content))
                AddBrokenRule(new BusinessRule(nameof(Content), "消息内容不能为空！"));
        }
    }
}
