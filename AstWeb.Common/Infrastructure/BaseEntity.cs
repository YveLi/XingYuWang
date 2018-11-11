using System.Collections.Generic;

namespace AstWeb.Common.Infrastructure
{
    /// <summary>
    /// Base class for entities
    /// </summary>
    public abstract class BaseEntity<TId>
    {
        private readonly List<BusinessRule> _brokenRules = new List<BusinessRule>();

        public TId Id { get; set; }
        /// <summary>
        /// 验证
        /// </summary>
        protected abstract void Validate();
        /// <summary>
        /// 获取破坏的规则
        /// </summary>
        /// <returns>规则列表</returns>
        public IEnumerable<BusinessRule> GetBrokenRules()
        {
            _brokenRules.Clear();
            Validate();
            return _brokenRules;
        }
        /// <summary>
        /// 添加规则
        /// </summary>
        /// <param name="businessRule">规则列表</param>
        protected void AddBrokenRule(BusinessRule businessRule)
        {
            _brokenRules.Add(businessRule);
        }
        public override bool Equals(object entity)
        {
            return entity != null && entity is BaseEntity<TId> && this == (BaseEntity<TId>)entity;
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        public static bool operator ==(BaseEntity<TId> entity1,
           BaseEntity<TId> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
            {
                return true;
            }

            if ((object)entity1 == null || (object)entity2 == null)
            {
                return false;
            }

            if (entity1.Id.ToString() == entity2.Id.ToString())
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(BaseEntity<TId> entity1,
            BaseEntity<TId> entity2)
        {
            return (!(entity1 == entity2));
        }
    }
}
