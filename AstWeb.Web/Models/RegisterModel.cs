using AstWeb.Common.Models;
using System;

namespace AstWeb.Models
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public int IsDaylight { get; set; }
        public Gender Gender { get; set; }
        public string Pass { get; set; }
        public string NickName { get; set; }
        public string Mobile { get; set; }
        public string place1 { get; set; }
        public string place2 { get; set; }
        public string PhoneNumber { get; set; }
        public string SmsCode { get; set; }
        public string AccessToken { get; set; }
        public string HeadPortrait { get; set; }
    }
    public class ActivityModel
    {
        public int activityid { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string ImgUrl { get; set; }
        public string Mobile { get; set; }
        public string Memo { get; set; }
    }
}