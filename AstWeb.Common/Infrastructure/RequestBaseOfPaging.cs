namespace AstWeb.Common.Infrastructure
{
    public class RequestBaseOfPaging
    {
        public RequestBaseOfPaging(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
    }
}
