using AstWeb.Common.Infrastructure;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Models
{
    [TableName("W_StarTest")]
    [PrimaryKey("Id")]
    public class StarTest : BaseEntity<int>
    {
        public string Url { get; set; }
        public string Color { get; set; }
        public string BackgroundColor { get; set; }
        public string LogoSrc { get; set; }
        public string Name { get; set; }
        public string Developer { get; set; }
        public int Times { get; set; }
        public int Like { get; set; }
        public string PraisePer { get; set;}
        public string Intro { get; set; }
        public int Price { get; set; }
        public string SlideImg { get; set; }
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
