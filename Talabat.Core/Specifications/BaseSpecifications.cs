using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
       

        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get ; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }

        // Get All
        public BaseSpecifications()
        {
            //Includes = new List<Expression<Func<T, object>>>();
        }
        public BaseSpecifications(Expression<Func<T, bool>> CriteriaExperssion)
        {
            Criteria = CriteriaExperssion;
            //Includes = new List<Expression<Func<T, object>>>(); 
        }

        public void AddOrderBy (Expression<Func<T, object>> OrderByExpression)
        {
            OrderBy = OrderByExpression;
        }
        public void AddOrderByDesc(Expression<Func<T, object>> OrderByDescExpression)
        {
            OrderByDescending = OrderByDescExpression;
        }


        public void ApplyPagination(int skip,int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
