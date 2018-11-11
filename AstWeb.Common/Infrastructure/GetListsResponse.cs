using System.Collections.Generic;

namespace AstWeb.Common.Infrastructure
{
    public class GetListsResponse<T> : ResponseBase
    {
        public IList<T> Items { get; set; }
    }
}
