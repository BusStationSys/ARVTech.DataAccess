namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.Destin
{
    using ARVTech.DataAccess.Domain.Entities.Destin;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;
    using System;

    /// <summary>
    /// Repository interface for managing "Concurso" entities.
    /// </summary>
    public interface IConcursoRepository : ICreateRepository<ConcursoEntity>, IReadRepository<ConcursoEntity, Guid>, IUpdateRepository<ConcursoEntity, Guid, ConcursoEntity>, IDeleteRepository<Guid>
    {
        ConcursoEntity GetByIdModalidadeAndNumeroAndDataApuracao(int idModalidade, int numero, DateTime dataApuracao);
    }
}