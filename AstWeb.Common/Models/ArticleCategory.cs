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
    /// 文章分类
    /// </summary>
    [TableName("W_ArticleCategories")]
    [PrimaryKey("Id")]
    public class ArticleCategory : BaseEntity<int>
    {
        public int ParentId { get; set; }
        public string CategoryName { get; set; }
        public string Memo { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
