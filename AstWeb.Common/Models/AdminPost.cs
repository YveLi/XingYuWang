using AstWeb.Common.Extension;
using AstWeb.Common.Infrastructure;
using AstWeb.Common.Services;
using NPoco;
using System;

namespace AstWeb.Common.Models
{
    /// <summary>
    /// 超级用户发帖
    /// </summary>
    public class AdminPost : BaseEntity<int>
    {
        public string NickName { get; set; }
        /// <summary>
        /// 称号
        /// </summary>
        public string Title { get; set; }
        public int PostCateogryId { get; set; }
        public string Content { get; set; }
        public int Reward { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPortrait { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; set; }
        /// <summary>
        /// 是否VIP
        /// </summary>
        public int IsVip { get; set; }
        /// <summary>
        /// VIP等级
        /// </summary>
        public int VipLevel { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否已禁用
        /// </summary>
        public int IsDisabled { get; set; }
        /// <summary>
        /// 邮箱是否可修改
        /// </summary>
        public int EmailIsUpdate { get; set; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public int IsAdmin { get; set; }

        /// <summary>
        /// 关注数
        /// </summary>
        public int LikeNum { get; set; }

        /// <summary>
        /// 访问数
        /// </summary>
        public int VisitorsNum { get; set; }

        /// <summary>
        /// 占星师等级
        /// </summary>
        public int AsterLevel { get; set; }
        /// <summary>
        /// 微博|微博号
        /// </summary>
        public string WeiBo { get; set; }
        /// <summary>
        /// 英文名字
        /// </summary>
        public string EName { get; set; }


        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
