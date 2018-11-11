using AstWeb.Common.Models;
using System;
using AstWeb.Common.Extension;
using AstWeb.Common.Infrastructure;
using AstWeb.Common.Services.Messaging;
using System.Collections.Generic;

namespace AstWeb.Common.Services
{
    public class ActivtyListService : BaseService<ActivtyList, int>
    {
        public ActivtyList GetActivtyInfo(int id)
        {
            return DbBase.SingleOrDefaultById<ActivtyList>(id);
        }

        /// <summary>
        /// 访问次数+10的随机数
        /// </summary>
        /// <param name="entity"></param>
        public void VisitPlusOne(ActivtyList entity)
        {
            if (entity != null)
            {
                Random r = new Random();
                entity.visit += r.Next(1, 11);
                DbBase.UpdateMany<ActivtyList>().OnlyFields(p => p.visit).Where(p => p.Id == entity.Id).Execute(entity);
            }
        }
    }
}
