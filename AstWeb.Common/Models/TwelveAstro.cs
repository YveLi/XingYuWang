using AstWeb.Common.Infrastructure;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Models
{
    [TableName("TwelveAstro")]
    [PrimaryKey("Id")]
    public class TwelveAstro : BaseEntity<int>
    {
        public string Traits { get; set; }
        public string PlacePosition { get; set; }
        public string YinYang { get; set; }
        public string MaxTraits { get; set; }
        public string MainPosition { get; set; }
        public string Color { get; set; }
        public string Gem { get; set; }
        public string LuckyNum { get; set; }
        public string Metal { get; set; }
        public string Memo { get; set; }
        public string AstroImg { get; set; }
        public string MaleTraits { get; set; }
        public string MaleWeakness { get; set; }
        public string MaleLove { get; set; }
        public string MaleMemo { get; set; }
        public string FemaleTraits { get; set; }
        public string FemaleWeakness { get; set; }
        public string FemaleLove { get; set; }
        public string FemaleMemo { get; set; }
        public int CreateUser { get; set; }
        public int ModifyUser { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public string AstroName { get; set; }
        public string AstroMonthDay { get; set; }
        public string AstroAttr { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        protected override void Validate()
        {
        }
    }
}
