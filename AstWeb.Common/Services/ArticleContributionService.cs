using AstWeb.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Services
{
   public class ArticleContributionService : BaseService<ArticleContribution, int>
    {
        //贡献+1
        public void AddContribution(ArticleContribution entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            ThrowExceptionIfEntityIsInvalid(entity);
            try
            {
                var con = DbBase.Query<ArticleContribution>().SingleOrDefault(p => p.UserId == entity.UserId && p.Time == DateTime.Now.ToString("yyyy-MM"));
                if (con != null)
                {
                    con.Number++;
                    DbBase.UpdateMany<ArticleContribution>().OnlyFields(p => p.Number).Where(p => p.Id == con.Id).Execute(con);
                    return;
                }
                DbBase.Insert(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
