using AstWeb.Common.Infrastructure;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Models
{
    [TableName("ActivityUser")]
    [PrimaryKey("Id")]
    public class ActivityUser : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string Mobile { get; set; }
        public string ImgUrl { get; set; }
        public int voteNum { get; set; }
        public int ActivityId { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime ModilyTime { get; set; }
        public string Memo { get; set; }
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
