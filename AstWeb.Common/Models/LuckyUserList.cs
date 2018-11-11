using AstWeb.Common.Infrastructure;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Models
{

    [TableName("LuckyUserList")]
    [PrimaryKey("Id")]
    public class LuckyUserList : BaseEntity<int>
    {
        public int ActivityId { get; set; }
        public string WechatId { get; set; }
        public int Wid { get; set; }
        public int LuckId { get; set; }
        public int Lv { get; set; }
        public int AfterCount { get; set; }
        public int BeforCount { get; set; }
        public string GUID { get; set; }
        public string LuckName { get; set; }
        public string Memo { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime ModifyTime { get; set; }

        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}

