namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ARVTech.DataAccess.Domain.Common;

    public interface IReadRepository<T, Y> where T : class
    {
        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        T Get(Y id);

        Task<T> GetAsync(Y id);

        PagedResult<T> GetAllPaged(int pageNumber, int pageSize);

        Task<PagedResult<T>> GetAllPagedAsync(int pageNumber, int pageSize);
    }
}