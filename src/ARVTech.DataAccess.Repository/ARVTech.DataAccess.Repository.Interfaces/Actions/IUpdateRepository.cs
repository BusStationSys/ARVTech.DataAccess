namespace ARVTech.DataAccess.Repository.Interfaces.Actions
{
    public interface IUpdateRepository<T> where T : class
    {
        T Update(T entity);
    }
}