using AstWeb.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Services
{
   public class PostContributionService:BaseService<PostContribution, int>
    {
        public void AddContribution(PostContribution entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            ThrowExceptionIfEntityIsInvalid(entity);
            try
            {
                //var con = DbBase.Query<PostContribution>().SingleOrDefault(p => p.UserId == entity.UserId && p.Time == DateTime.Now.ToString("yyyy-MM"));
                //if (con != null)
                //{
                //    con.Number++;
                //    DbBase.UpdateMany<PostContribution>().OnlyFields(p => p.Number).Where(p => p.Id == con.Id).Execute(con);
                //    return;
                //}
                DbBase.Insert(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
