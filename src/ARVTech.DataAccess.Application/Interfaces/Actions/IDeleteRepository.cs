using System.Linq.Expressions;

namespace ARVTech.DataAccess.Application.Interfaces.Actions
{
    public interface IDeleteRepository<Y>
    {
        void Delete(Y id);

        void DeleteMany(Expression<Func<Y, bool>> filter);
    }
}