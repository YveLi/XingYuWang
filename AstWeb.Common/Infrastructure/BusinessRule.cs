namespace AstWeb.Common.Infrastructure
{
    /// <summary>
    /// 业务规则
    /// </summary>
    public class BusinessRule
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="property">属性</param>
        /// <param name="rule">规则</param>
        public BusinessRule(string property, string rule)
        {
            Property = property;
            Rule = rule;
        }
        /// <summary>
        /// 属性
        /// </summary>
        public string Property { get; set; }
        /// <summary>
        /// 规则
        /// </summary>
        public string Rule { get; set; }
    }
}

