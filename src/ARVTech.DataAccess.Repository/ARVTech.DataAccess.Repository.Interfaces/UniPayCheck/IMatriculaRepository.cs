namespace ARVTech.DataAccess.Repository.Interfaces.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaRepository : ICreateRepository<MatriculaEntity>, IReadRepository<MatriculaEntity, Guid>, IUpdateRepository<MatriculaEntity>, IDeleteRepository<Guid>
    {
        void DeleteDemonstrativoPagamento(Guid guidMatricula, Guid guid);

        void DeleteEspelhoPonto(Guid guidMatricula, Guid guid);
    }
}