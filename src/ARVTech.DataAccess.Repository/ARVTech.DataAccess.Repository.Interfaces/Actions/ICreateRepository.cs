namespace ARVTech.DataAccess.Repository.Interfaces.Actions
{
    public interface ICreateRepository<T> where T : class
    {
        T Create(T entity);
    }
}