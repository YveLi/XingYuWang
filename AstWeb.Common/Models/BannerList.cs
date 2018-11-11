using AstWeb.Common.Infrastructure;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Models
{
    [TableName("W_BannerList")]
    [PrimaryKey("Id")]
    public class BannerList : BaseEntity<int>
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public int Sort { get; set; }
        public string Memo { get; set; }
        public int ModuleType { get; set; }
        protected override void Validate()
        {
        }
    }
}
