using AstWeb.Common.Infrastructure;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Models
{
    [TableName("W_AsterLikes")]
    [PrimaryKey("Id")]
    public class AsterLikes : BaseEntity<int>
    {
        /// <summary>
        /// 关注Id
        /// </summary>
        public int LikeUserId { get; set; }
        /// <summary>
        /// 关注Id
        /// </summary>
        [Reference(ReferenceType.OneToOne, ColumnName = "LikeUserId", ReferenceMemberName = "Id")]
        public W_Users LikeUser { get; set; }
        [Reference(ReferenceType.OneToOne, ColumnName = "UserId", ReferenceMemberName = "Id")]
        public W_Users UserLike { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        protected override void Validate()
        {
            //throw new NotImplementedException();
        }
    }
}
