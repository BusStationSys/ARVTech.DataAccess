namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaEspelhoPontoRepository : ICreateRepository<MatriculaEspelhoPontoEntity>, IDeleteRepository<Guid>, IReadRepository<MatriculaEspelhoPontoEntity, Guid>, IUpdateRepository<MatriculaEspelhoPontoEntity, Guid, MatriculaEspelhoPontoEntity>
    {
        void DeleteCalculosAndMarcacoesByCompetenciaAndGuidMatricula(string competencia, Guid guidMatricula);

        IEnumerable<MatriculaEspelhoPontoEntity> Get(string competencia, string matricula);

        IEnumerable<MatriculaEspelhoPontoEntity> GetByGuidColaborador(Guid guidColaborador);

        (DateTime dataInicio, DateTime dataFim, int quantidadeRegistrosAtualizados, int quantidadeRegistrosInalterados, int quantidadeRegistrosInseridos, int quantidadeRegistrosRejeitados) ImportFileEspelhosPonto(string cnpj, string content);
    }
}