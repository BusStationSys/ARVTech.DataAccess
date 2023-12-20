namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions
{
    public interface ICreateRepository<T> where T : class
    {
        T Create(T entity);
    }
}