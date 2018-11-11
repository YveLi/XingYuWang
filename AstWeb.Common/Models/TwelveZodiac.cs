using AstWeb.Common.Infrastructure;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Models
{
    [TableName("TwelveZodiac")]
    [PrimaryKey("Id")]
    public class TwelveZodiac : BaseEntity<int>
    {
        public string ZodiacName { get; set; }
        public string FiveAttr { get; set; }
        public string LifeBuddha { get; set; }
        public string LuckyColor { get; set; }
        public string FierceColor { get; set; }
        public string LuckyNum { get; set; }
        public string FierceNum { get; set; }
        public string LuckyFlower { get; set; }
        public string LuckyPosition { get; set; }
        public string Memo { get; set; }
        public string ZodiacVirtues { get; set; }
        public string ZodiacFlaws { get; set; }
        public string Career { get; set; }
        public string Love { get; set; }
        public string Fortune { get; set; }
        public string Health { get; set; }
        public string GoodWith { get; set; }
        public string GoodWithMemo { get; set; }
        public string BadWith { get; set; }
        public string BadWithMemo { get; set; }
        public int CreateUser { get; set; }
        public int ModifyUser { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public string ZodiacImg { get; set; }
        public string ZodiaYear { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        protected override void Validate()
        {
        }
    }
}
