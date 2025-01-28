namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions
{
    public interface IUpdateRepository<out T, in U, in V> where T : class
    {
        T Update(U id, V entity);
    }
}