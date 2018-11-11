using AstWeb.Common.Infrastructure;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Models
{
    [TableName("BannerList")]
    [PrimaryKey("Id")]
    public class ActivityBannerList : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Img { get; set; }
        public string Memo { get; set; }
        public int Sort { get; set; }
        public DateTime AddTime { get; set; }
        protected override void Validate()
        {
        }
    }
}
