

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        #region Without specifications
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);


        Task AddAsync(T Item);
        void Update(T Item);
        void Delete(T Item);

        #endregion



        #region With specifications
        Task<IReadOnlyList<T>> GetAllAsyncWithSpecifications(ISpecifications<T> Spec);
        Task<T> GetEntityAsyncWithSpecifications(ISpecifications<T> Spec); 

        Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec);

        #endregion

        

    }
}
