using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {

        // signature for property for where condition
        // take t and return bool
        public Expression<Func<T, bool>> Criteria { get; set; }

        // signature for property for list of include
        public List<Expression<Func<T,object>>> Includes { get; set; }

        // prop for order by 
        public Expression<Func<T,object>> OrderBy { get; set; }
        // prop for order by descending
        public Expression<Func<T, object>> OrderByDescending { get; set; }

        // Take
        public int Take { get; set; }

        // Skip
        public int Skip { get; set; }

        public bool IsPaginationEnabled { get; set; }
    }
}
