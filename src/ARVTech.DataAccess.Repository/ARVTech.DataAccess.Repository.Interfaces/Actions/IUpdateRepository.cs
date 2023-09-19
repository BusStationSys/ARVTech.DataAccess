namespace ARVTech.DataAccess.Repository.Interfaces.Actions
{
    //public interface IUpdateRepository<T> where T : class
    //{
    //    T Update(T entity);
    //}

    public interface IUpdateRepository<out T, in U, in V> where T : class
    {
        T Update(U pk, V entity);
    }
}