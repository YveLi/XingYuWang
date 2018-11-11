using NPoco;

namespace AstWeb.Common.Infrastructure
{
    public class GetPagingResponse<T>: ResponseBase
    {
        public Page<T> Pages { get; set; }
    }

}
