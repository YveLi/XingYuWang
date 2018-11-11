using AstWeb.Common.Extension;
using AstWeb.Common.Infrastructure;
using AstWeb.Common.Services;
using NPoco;
using System;

namespace AstWeb.Common.Models
{
    /// <summary>
    /// 用户实体
    /// </summary>
    [TableName("W_Users")]
    [PrimaryKey("Id")]
    public class W_Users : BaseEntity<int>
    {
        /// <summary>
        /// 星座
        /// </summary>
        [Ignore]
        public string Xingzuo
        {
            get
            {
                int[] dayArr = new int[] { 20, 19, 21, 20, 21, 22, 23, 23, 23, 24, 23, 22 };
                String[] constellationArr = new String[] { "摩羯座", "水瓶座", "双鱼座", "白羊座", "金牛座", "双子座", "巨蟹座", "狮子座", "处女座", "天秤座", "天蝎座", "射手座", "摩羯座" };
                int month = int.Parse(Birthday.ToString("MM"));
                int day = int.Parse(Birthday.ToString("dd"));
                return day < dayArr[month - 1] ? constellationArr[month - 1] : constellationArr[month]; ;
            }
        }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 是否夏令时
        /// </summary>
        public int IsDaylight { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 邮箱是否已验证
        /// </summary>
        public int EmailConfirmed { get; set; }
        /// <summary>
        /// 邮箱验证Token
        /// </summary>
        public string EmailConfirmToken { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 称号
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string Place1 { get; set; }
        public string Place2 { get; set; }
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
        public string AccessToken { get; set; }
      
        #region 占星师信息
        /// <summary>
        /// qq营销
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 是否占星师
        /// </summary>
        public int IsAster { get; set; }

        /// <summary>
        /// 占星师排名
        /// </summary>
        public int AsterSort { get; set; }
        /// <summary>
        /// 占星师封面
        /// </summary>
        public string AsterImg { get; set; }

        /// <summary>
        /// 占星师标签
        /// </summary>
        public string AsterTag { get; set; }

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
        #endregion
        ///// <summary>
        ///// 该用户发表的帖子总数
        ///// </summary>
        //[Ignore]
        //public int PostCount
        //{
        //    get
        //    {
        //        return new PostService().GetPostCountByUserId(Id);
        //    }
        //}

        protected override void Validate()
        {
            var _service = new UserService();

            //if (string.IsNullOrEmpty(Email))
            //    AddBrokenRule(new BusinessRule("Email", "邮箱地址不能为空."));
            //if (!Email.IsEmail())
            //    AddBrokenRule(new BusinessRule("Email", "邮箱地址不正确."));
            //if (Id == 0)
            //{
            //    if (_service.CheckEmailIsExists(Email))
            //        AddBrokenRule(new BusinessRule("Email", "邮箱地址已存在."));
            //}
            //if (string.IsNullOrEmpty(NickName))
            //    AddBrokenRule(new BusinessRule("Email", "昵称不能为空."));
            if (Id == 0)
            {
                if (_service.CheckNicknameIsExists(NickName))
                    AddBrokenRule(new BusinessRule("Nickname", "昵称已存在."));
            }

            if (string.IsNullOrEmpty(Password))
                AddBrokenRule(new BusinessRule("Password", "密码不能为空."));
        }
    }
}
