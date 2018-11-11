using AstWeb.Common.Helper;
using AstWeb.Common.Infrastructure;
using NPoco;
using System;

namespace AstWeb.Common.Models
{
    /// <summary>
    /// 访客实体
    /// </summary>
    [TableName("W_Visitors")]
    [PrimaryKey("Id")]
    public class Visitor : BaseEntity<int>
    {
        /// <summary>
        /// 访客Id
        /// </summary>
        public int VisitorUserId { get; set; }
        [Reference(ReferenceType.OneToOne, ColumnName = "VisitorUserId", ReferenceMemberName = "Id")]
        public W_Users VisitorUser { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 最后到访时间
        /// </summary>
        public DateTime LastTime { get; set; }

        /// <summary>
        /// 获取文字描述的时间
        /// </summary>
        [Ignore]
        public string TimeAgo
        {
            get
            {
                return DateHelper.TimeAgoStr(DateTime.Now, LastTime);
            }
        }


        protected override void Validate()
        {
        }
    }
}
