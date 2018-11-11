using AstWeb.Common.Infrastructure;
using System;
using NPoco;
using AstWeb.Common.Helper;
namespace AstWeb.Common.Models
{
    /// <summary>
    /// 漫画类型实体
    /// </summary>
    [TableName("W_CartoonCategories")]
    [PrimaryKey("Id")]
    public class CartoonCategories : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Memo { get; set; }
        public int Sort { get; set; }
        public string TopImg { get; set; }
        public string BackImg { get; set; }
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
