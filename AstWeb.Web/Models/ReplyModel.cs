namespace AstWeb.Models
{
    public class ReplyModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId{get;set;}
    }
}