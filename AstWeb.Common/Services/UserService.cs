using AstWeb.Common.Infrastructure;
using AstWeb.Common.Models;
using System;
using System.Text;
using AstWeb.Common.Extension;
using AstWeb.Common.Email;
using AstWeb.Common.Configuration;

namespace AstWeb.Common.Services
{
    public class UserService : BaseService<W_Users, int>
    {
        /// <summary>
        /// 发送激活邮件
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Response SendActivateEmail(string email)
        {
            var response = new Response();
            var user = DbBase.Query<W_Users>().SingleOrDefault(p => p.Email == email);
            if (user == null)
            {
                response.Message = "发送失败,请重新登录后重试.";
                return response;
            }
            user.EmailConfirmToken = (email + DateTime.Now.Ticks).ToMd5().ToLower();
            var result = DbBase.UpdateMany<W_Users>().OnlyFields(p => p.EmailConfirmToken).Where(p => p.Id == user.Id).Execute(user);
            if (IsUpdateSuccess(result))
            {
                var url = WebConfigApplicationSettings.GetAppSettings("Domain", "http://www.byr8.top/") + "emailvalidate/" + user.EmailConfirmToken;
                var body = new StringBuilder();
                body.Append("您好：" + user.NickName + "<br/>");
                body.Append("<p>请点击链接激活您的邮箱：");//target=\"_blank\"
                body.Append("<a href=\"" + url + "\"  >" + url + "</a>");
                body.Append("</p>");

                //发送邮件
                new SmtpService().SendMail(email, "(勿回)星座社区激活邮件", body.ToString());

                response.IsSuccess = true;
                response.Message = "发送成功,请注意查看邮件.";
                return response;
            }

            response.Message = "发送失败,请重新登录后重试.";
            return response;
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="token">token</param>
        /// <returns></returns>
        public Response EmailValidate(string token)
        {
            var response = new Response();
            if (string.IsNullOrEmpty(token))
            {
                response.Message = "验证失败：信息无效，请登录重新发送激活链接.";
                return response;
            }
            var user = DbBase.Query<W_Users>().SingleOrDefault(p => p.EmailConfirmToken == token);
            if (user == null)
            {
                response.Message = "验证失败：信息无效，请登录重新发送激活链接.";
                return response;
            }
            user.EmailConfirmed = 1;
            user.EmailIsUpdate = 0;
            var result = DbBase.Update(user);
            if (IsUpdateSuccess(result))
            {
                response.IsSuccess = true;
                response.Message = "邮箱验证成功，帐号可以正常使用啦.";
                return response;
            }

            response.Message = "验证失败：信息无效，请登录重新发送激活链接.";
            return response;
        }
        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="headPortrait"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public void UpdateHeadPortrait(string headPortrait, int userId)
        {
            var response = new Response();

            DbBase.UpdateMany<W_Users>().OnlyFields(p => p.HeadPortrait).Where(p => p.Id == userId).Execute(new W_Users
            {
                HeadPortrait = headPortrait
            });
        }
        public void UpdatePwd(string pwd, int userId)
        {
            var response = new Response();
            DbBase.UpdateMany<W_Users>().OnlyFields(p => p.Password).Where(p => p.Id == userId).Execute(new W_Users
            {
                Password = pwd.ToMd5()
            });
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public W_Users Login(string email)
        {
            return DbBase.Query<W_Users>().SingleOrDefault(p => p.Email == email || p.PhoneNumber == email);
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public W_Users GetUserInfo(int userId)
        {
            return DbBase.SingleOrDefaultById<W_Users>(userId);
        }

        public int GetUserCount()
        {
            return DbBase.Query<W_Users>().Count(p => true);
        }
        public int GetAsterCount()
        {
            return DbBase.Query<W_Users>().Count(p => p.IsAster == 1);
        }
        /// <summary>
        ///通过同户名 获取用户信息
        /// </summary>
        /// <param name="nickname">昵称</param>
        /// <returns></returns>
        public W_Users GetUserByNickname(string nickname)
        {
            return DbBase.Query<W_Users>().SingleOrDefault(p => p.NickName == nickname);
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Response UpdateUser(W_Users entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            ThrowExceptionIfEntityIsInvalid(entity);

            var response = new Response();
            var result = DbBase.Update(entity);
            if (IsInsertSuccess(result))
            {
                response.IsSuccess = true;
                response.Message = "更新成功！";
                return response;
            }

            response.Message = "更新失败，请检查！";
            return response;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Response Register(W_Users entity)
        {
            var response = new Response();
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));
                ThrowExceptionIfEntityIsInvalid(entity);
                entity.CreateTime = DateTime.Now;//确保有创建时间，防止报错
                var result = DbBase.Insert(entity);
                if (IsInsertSuccess(result))
                {
                    response.IsSuccess = true;
                    response.Message = "注册成功！";
                    return response;
                }
                response.Message = "注册失败，请检查！";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        /// <summary>
        /// 检查手机号是否已存在
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public bool CheckPhoneExists(string phone, int? userId = null)
        {
            if (userId != null)
                return DbBase.Query<W_Users>().Any(p => p.PhoneNumber == phone && p.Id != userId);
            return DbBase.Query<W_Users>().Any(p => p.PhoneNumber == phone);
        }
        /// <summary>
        /// 检查邮箱是否已存在
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool CheckEmailIsExists(string email, int? userId = null)
        {
            if (email == "" || email == null) { return false; }
            if (userId != null)
                return DbBase.Query<W_Users>().Any(p => p.Email == email && p.Id != userId);
            return DbBase.Query<W_Users>().Any(p => p.Email == email);
        }
        public W_Users CheckQQ(string accesstoken)
        {
            return DbBase.Query<W_Users>().FirstOrDefault(p => p.AccessToken == accesstoken);
        }
        /// <summary>
        /// 检查昵称是否已存在
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public bool CheckNicknameIsExists(string nickname, int? userId = null)
        {
            if (userId != null)
                return DbBase.Query<W_Users>().Any(p => p.NickName == nickname && p.Id != userId);
            return DbBase.Query<W_Users>().Any(p => p.NickName == nickname);
        }

        #region 占星师
        public GetListsResponse<W_Users> GetAsterListToHome()
        {
            var response = new GetListsResponse<W_Users>();
            var result = DbBase.Query<W_Users>().OrderBy(p => p.AsterSort).Where(u => u.IsAster == 1).ToPage(1, 4);
            if (result.Items != null && result.Items.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "获取成功";
                response.Items = result.Items;
                return response;
            }
            response.Message = "暂无数据";
            return response;
        }
        #endregion
        #region 判断手机号是否已被注册
        public bool IsExist(string mobile)
        {
            var result = DbBase.Query<W_Users>().Count(u => u.Email == mobile);
            if(result > 0){
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
