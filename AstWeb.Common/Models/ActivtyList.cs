using AstWeb.Common.Infrastructure;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Models
{
    [TableName("ActivtyList")]
    [PrimaryKey("Id")]
    public class ActivtyList : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string HostUrl { get; set; }
        public int visit { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string AcContent { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public DateTime BmTime { get; set; }
        public DateTime BmEndTime { get; set; }
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
