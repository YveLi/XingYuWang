using AstWeb.Common.Infrastructure;
using System;
using NPoco;
using AstWeb.Common.Helper;
namespace AstWeb.Common.Models
{
    /// <summary>
    /// 漫画实体
    /// </summary>
    [TableName("W_StarCartoon")]
    [PrimaryKey("Id")]
    public class StarCartoon : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Content { get; set; }

        //取得对应的漫画类型名
        [Reference(ReferenceType.OneToOne, ColumnName = "CartoonCategoryId", ReferenceMemberName = "Id")]
        public CartoonCategories Category { get; set; }
        public string ImgUrl { get; set; }
        public int IsShow { get; set; }
        public int IsTop { get; set; }
        public DateTime AddTime { get; set; }
        public int Sort { get; set; }
        public int Hits { get; set; }
        public int TUserId { get; set; }
        public int CommentCount { get; set; }

        [Reference(ReferenceType.OneToOne, ColumnName = "TUserId", ReferenceMemberName = "Id")]

        public W_Users User { get; set; }
        public int CartoonCategoryId { get; set; }
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
