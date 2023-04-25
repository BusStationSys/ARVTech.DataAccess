namespace ARVTech.DataAccess.Repository.Interfaces.EquHos
{
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    public interface IRecursoRepository : ICreateRepository<RecursoEntity>, IReadRepository<RecursoEntity, int>, IUpdateRepository<RecursoEntity>, IDeleteRepository<int>
    {
    }
}