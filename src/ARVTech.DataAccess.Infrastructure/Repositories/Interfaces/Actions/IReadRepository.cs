namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IReadRepository<T, Y> where T : class
    {
        IEnumerable<T> GetAll();

        T Get(Y id);

        IEnumerable<T> GetMany(Expression<Func<T, bool>> filter = null,
                                               Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                               int? top = null,
                                               int? skip = null,
                                               params string[] includeProperties);
    }
}