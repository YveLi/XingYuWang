using AstWeb.Common.Infrastructure;
using NPoco;
using System;

namespace AstWeb.Common.Models
{
    /// <summary>
    /// 帖子类型
    /// </summary>
    [TableName("W_PostCategories")]
    [PrimaryKey("Id")]
    public class PostCategory : BaseEntity<int>
    {
        /// <summary>
        /// 帖子名称
        /// </summary>
        public string CategoryName { get; set; }

        public int ParentId { get; set; }
        public string Memo { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
