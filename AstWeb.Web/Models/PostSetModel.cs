namespace AstWeb.Models
{
    public class PostSetModel
    {
        /// <summary>
        /// 帖子ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 要设置的值
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Field { get; set; }

    }
}