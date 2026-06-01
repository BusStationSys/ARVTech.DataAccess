namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaRepository : ICreateRepository<MatriculaEntity>, IReadRepository<MatriculaEntity, Guid>, IUpdateRepository<MatriculaEntity, Guid, MatriculaEntity>, IDeleteRepository<Guid>
    {
        void DeleteEspelhosPonto(Guid guidMatricula);

        IEnumerable<MatriculaEntity> GetAniversariantesEmpresa(int mes);

        MatriculaEntity GetByMatricula(string matricula);

        (DateTime dataInicio, DateTime dataFim, int quantidadeRegistrosAtualizados, int quantidadeRegistrosInalterados, int quantidadeRegistrosInseridos) ImportFileMatriculas(string cnpj, string content);

        (DateTime dataInicio, DateTime dataFim, int quantidadeRegistrosAtualizados, int quantidadeRegistrosInalterados, int quantidadeRegistrosInseridos) SincronizarCredenciaisMatriculasUsuarios(Guid? guidColaborador = null, DateTime? dataInclusao = null, DateTime? dataUltimaAlteracao = null);
    }
}