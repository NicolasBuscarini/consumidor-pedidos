namespace ConsumidorPedidos.Data.MySql.Repository.Interface
{
    public interface IGenericRepository<T, TKey>
        where T : class
        where TKey : struct
    {
        Task<T> CreateAsync(T entity);
        Task<bool> DeleteAsync(T entity, TKey id);
        T GetById(TKey id);
        Task<T> GetByIdAsync(TKey id);
        Task<T> GetByIdIncludingAsync(TKey id, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<int> UpdateAsync(T entity, TKey id);
        IQueryable<T> ListAll();
        IQueryable<T> ListAll(int pageNumber, int pageSize);
    }
}