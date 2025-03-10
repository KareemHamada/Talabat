﻿

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;
        public GenericRepository(StoreContext dbContext) {
            _dbContext = dbContext;
        }


        #region Without specifications
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
                return await _dbContext.Set<T>().ToListAsync();
        }



        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        #endregion 

        #region With specifications
        public async Task<IReadOnlyList<T>> GetAllAsyncWithSpecifications(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).ToListAsync();
        }

        public async Task<T> GetEntityAsyncWithSpecifications(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).FirstOrDefaultAsync();
        } 
        #endregion


        private IQueryable<T> ApplySpecification(ISpecifications<T> Spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), Spec);  
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).CountAsync();
        }

        public async Task AddAsync(T Item)
        {
            await _dbContext.Set<T>().AddAsync(Item);
        }

        public void Update(T Item)
        {
            _dbContext.Set<T>().Update(Item);
        }

        public void Delete(T Item)
        {
            _dbContext.Set<T>().Remove(Item);
        }
    }
}
