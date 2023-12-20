namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions
{
    using System.Linq.Expressions;

    public interface IDeleteRepository<Y>
    {
        void Delete(Y id);

        void DeleteMany(Expression<Func<Y, bool>> filter);
    }
}