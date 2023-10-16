namespace ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Application.Interfaces.Actions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaRepository : ICreateRepository<MatriculaEntity>, IReadRepository<MatriculaEntity, Guid>, IUpdateRepository<MatriculaEntity, Guid, MatriculaEntity>, IDeleteRepository<Guid>
    {
        void DeleteEspelhosPonto(Guid guidMatricula);

        MatriculaEntity GetByMatricula(string matricula);
    }
}