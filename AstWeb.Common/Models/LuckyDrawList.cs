using AstWeb.Common.Infrastructure;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Models
{
    [TableName("LuckyDrawList")]
    [PrimaryKey("Id")]
    public class LuckyDrawList : BaseEntity<int>
    {
        public int ActivityId { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Value { get; set; }
        public decimal Probability { get; set; }
        public int Count { get; set; }
        public string Memo{ get; set; }
        public DateTime AddTime { get; set; }
        public DateTime ModifyTime { get; set; }

        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
