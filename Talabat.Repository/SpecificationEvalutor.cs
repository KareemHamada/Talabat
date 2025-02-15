
namespace Talabat.Repository
{
    public static class SpecificationEvalutor<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> Spec)
        {
            var Query = inputQuery;

            // Where
            if (Spec.Criteria is not null) { 
            
                Query = Query.Where(Spec.Criteria);
            }
            // order by
            if (Spec.OrderBy is not null) { 
            
                Query = Query.OrderBy(Spec.OrderBy);
            }
            // order by Desc

            if (Spec.OrderByDescending is not null)
            {

                Query = Query.OrderByDescending(Spec.OrderByDescending);
            }

            if (Spec.IsPaginationEnabled)
            {
                Query = Query.Skip(Spec.Skip).Take(Spec.Take);
            }

            // includes
            Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));

            return Query;
        }   
    }
}
