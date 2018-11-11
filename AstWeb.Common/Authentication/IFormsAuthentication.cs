﻿namespace AstWeb.Common.Authentication
{
    /// <summary>
    /// Form身份验证接口
    /// </summary>
    public interface IFormsAuthentication
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="token"></param>
        void SetAuthenticationToken(string token, bool createPersistentCookie = false);
        /// <summary>
        /// 登出
        /// </summary>
        void SignOut();
    }
}
