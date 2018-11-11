using NPoco;
using AstWeb.Common.Extension;
using AstWeb.Common.Infrastructure;

namespace AstWeb.Common.Models
{
    /// <summary>
    /// 友情链接
    /// </summary>
    [TableName("W_Links")]
    [PrimaryKey("Id")]
    public class Links : BaseEntity<int>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        protected override void Validate()
        {
            if (string.IsNullOrEmpty(Name))
                AddBrokenRule(new BusinessRule("Name", "名称不能为空"));
            if (string.IsNullOrEmpty(Url))
                AddBrokenRule(new BusinessRule("Url", "链接不能为空"));
            if (!Url.IsUrl())
                AddBrokenRule(new BusinessRule("Url", "链接地址不合法"));
        }
    }
}
