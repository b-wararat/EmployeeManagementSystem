using BaseLibrary.Responses;

namespace ServerLibrary.Repositoies.Contracts
{
    public interface IGenericRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<GeneralResponse> Insert(T entity);
        Task<GeneralResponse> Update(T entity);
        Task<GeneralResponse> DeleteById(int id);
    }
}
