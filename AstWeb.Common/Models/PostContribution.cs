using AstWeb.Common.Infrastructure;
using NPoco;
using System;

namespace AstWeb.Common.Models
{
    /// <summary>
    /// 星币流水
    /// </summary>
    [TableName("W_PostContributions")]
    [PrimaryKey("Id")]
    public class PostContribution : BaseEntity<int>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        [Reference(ReferenceType.OneToOne, ColumnName = "UserId", ReferenceMemberName = "Id")]

        public int PostId { get; set; }
        [Reference(ReferenceType.OneToOne, ColumnName = "PostId", ReferenceMemberName = "Id")]
        public W_Users User { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 星币
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        protected override void Validate()
        {

        }
    }
}
