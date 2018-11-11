using AstWeb.Common.Infrastructure;
using NPoco;
using System;

namespace AstWeb.Common.Models
{
    /// <summary>
    /// 验证码实体
    /// </summary>
    [TableName("SMS")]
    [PrimaryKey("Id")]
    public class SMS : BaseEntity<int>
    {
        public string mobile { get; set; }
        public string code{ get; set; }
        public DateTime AddTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public int type { get; set; }

        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
