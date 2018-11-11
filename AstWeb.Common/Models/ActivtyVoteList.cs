using AstWeb.Common.Infrastructure;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Models
{

    [TableName("ActivtyVoteList")]
    [PrimaryKey("Id")]
    public class ActivtyVoteList : BaseEntity<int>
    {
        public int wid { get; set; }
        public int PlayerId { get; set; }
        public string WechatId { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public int ActivityId { get; set; }
        public string NickName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public int Sex { get; set; }
        public DateTime SubscribeTime { get; set; }
        public string HeadImgUrl { get; set; }
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
